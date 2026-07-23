using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;
using Treps.PaymentOrchestration.Sdk.Resources;

namespace Treps.PaymentOrchestration.Sdk;

/// <summary>
/// Low-level HTTP client for the Treps Payment Orchestration API. Handles login and token
/// caching; use the <see cref="Payments"/>, <see cref="ThreeDSecure"/>, <see cref="HostedPage"/>,
/// <see cref="Query"/>, <see cref="Cards"/>, <see cref="PaymentLinks"/>, and
/// <see cref="Insurance"/> resource properties built on top of it for an endpoint-specific API.
/// </summary>
public sealed class TrepsClient : IDisposable
{
    private static readonly Dictionary<TrepsEnvironment, string> EnvironmentBaseUrls = new()
    {
        [TrepsEnvironment.Sandbox] = "https://poapi.treps.tr",
        [TrepsEnvironment.Production] = "https://api.treps.io",
    };

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly string _baseUrl;
    private readonly string _username;
    private readonly string _password;
    private readonly int _merchantId;
    private readonly int _tokenExpiryMarginMs;
    private readonly SemaphoreSlim _loginLock = new(1, 1);

    private (string AccessToken, long ExpiresAtMs)? _cachedToken;

    /// <summary>Sale, PreAuth, PostAuth, Refund, Void.</summary>
    public PaymentsResource Payments { get; }

    /// <summary>3D Secure init/complete.</summary>
    public ThreeDSecureResource ThreeDSecure { get; }

    /// <summary>Secure Payment Page (Hosted Page) create/query.</summary>
    public HostedPageResource HostedPage { get; }

    /// <summary>BIN lookup, transaction/order detail &amp; reporting, commissions, installments, saved cards.</summary>
    public QueryResource Query { get; }

    /// <summary>Save/update/remove a tokenized card.</summary>
    public CardResource Cards { get; }

    /// <summary>Shareable payment links (create/get/list).</summary>
    public PaymentLinkResource PaymentLinks { get; }

    /// <summary>Insurance-sector payments.</summary>
    public InsuranceResource Insurance { get; }

    /// <param name="username">Your Treps portal username.</param>
    /// <param name="password">Your Treps portal password.</param>
    /// <param name="merchantId">Your Treps merchant id.</param>
    /// <param name="environment">Defaults to <see cref="TrepsEnvironment.Sandbox"/>. Ignored if <paramref name="baseUrl"/> is set.</param>
    /// <param name="baseUrl">Overrides the environment preset with a custom API base URL.</param>
    /// <param name="tokenExpiryMarginMs">Milliseconds of safety margin subtracted from the token's expiry. Defaults to 30s.</param>
    /// <param name="httpClient">
    /// Advanced/testing hook to supply your own <see cref="HttpClient"/> (e.g. one built from
    /// <c>IHttpClientFactory</c>, or one with a fake handler in tests). If omitted, the client
    /// creates and owns its own instance.
    /// </param>
    public TrepsClient(
        string username,
        string password,
        int merchantId,
        TrepsEnvironment environment = TrepsEnvironment.Sandbox,
        string? baseUrl = null,
        int tokenExpiryMarginMs = 30_000,
        HttpClient? httpClient = null)
    {
        _username = username;
        _password = password;
        _merchantId = merchantId;
        _tokenExpiryMarginMs = tokenExpiryMarginMs;
        _baseUrl = baseUrl ?? EnvironmentBaseUrls[environment];

        _ownsHttpClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient();

        Payments = new PaymentsResource(this);
        ThreeDSecure = new ThreeDSecureResource(this);
        HostedPage = new HostedPageResource(this);
        Query = new QueryResource(this);
        Cards = new CardResource(this);
        PaymentLinks = new PaymentLinkResource(this);
        Insurance = new InsuranceResource(this);
    }

    /// <summary>Performs POST /api/auth and returns the raw response — most callers don't need this directly.</summary>
    public async Task<LoginResponseData> LoginAsync(CancellationToken cancellationToken = default)
    {
        var token = await EnsureLoggedInAsync(cancellationToken).ConfigureAwait(false);

        return new LoginResponseData
        {
            AccessToken = token.AccessToken,
            ExpireIn = token.ExpiresAtMs,
            Scheme = "Bearer",
            TokenPolicy = "apiuser",
        };
    }

    /// <summary>
    /// GET/POST/PUT/DELETE against the Treps API with an automatically-attached, auto-refreshed
    /// bearer token.
    /// </summary>
    public async Task<T> RequestAsync<T>(HttpMethod method, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        var token = await EnsureLoggedInAsync(cancellationToken).ConfigureAwait(false);

        using var request = new HttpRequestMessage(method, _baseUrl + path);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var parsed = TryDeserialize<ApiResponse<T>>(responseText);

        if (!response.IsSuccessStatusCode || parsed is null || parsed.Status == false)
        {
            throw new TrepsApiException(
                parsed?.Message ?? $"Treps API request failed with HTTP {(int)response.StatusCode}",
                (int)response.StatusCode,
                parsed?.Errors,
                parsed);
        }

        return parsed.Data!;
    }

    private async Task<(string AccessToken, long ExpiresAtMs)> EnsureLoggedInAsync(CancellationToken cancellationToken)
    {
        if (_cachedToken is { } cached && NowMs() < cached.ExpiresAtMs - _tokenExpiryMarginMs)
        {
            return cached;
        }

        await _loginLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_cachedToken is { } cachedAfterLock && NowMs() < cachedAfterLock.ExpiresAtMs - _tokenExpiryMarginMs)
            {
                return cachedAfterLock;
            }

            return await PerformLoginAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _loginLock.Release();
        }
    }

    private async Task<(string AccessToken, long ExpiresAtMs)> PerformLoginAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/api/auth")
        {
            Content = JsonContent.Create(
                new LoginRequest { Username = _username, Password = _password, MerchantId = _merchantId },
                options: JsonOptions),
        };

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var parsed = TryDeserialize<ApiResponse<LoginResponseData>>(responseText);

        if (!response.IsSuccessStatusCode || parsed is null || parsed.Status == false || parsed.Data is null)
        {
            throw new TrepsApiException(
                parsed?.Message ?? $"Treps login failed with HTTP {(int)response.StatusCode}",
                (int)response.StatusCode,
                parsed?.Errors,
                parsed);
        }

        var token = (parsed.Data.AccessToken, parsed.Data.ExpireIn);
        _cachedToken = token;
        return token;
    }

    private static TResult? TryDeserialize<TResult>(string json)
        where TResult : class
    {
        try
        {
            return JsonSerializer.Deserialize<TResult>(json, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static long NowMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public void Dispose()
    {
        _loginLock.Dispose();
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}
