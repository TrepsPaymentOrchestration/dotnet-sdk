using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Secure Payment Page (Hosted Page) and embedded IFrame checkout — both are the same
/// <c>/api/payment/hostedpage</c> endpoint, distinguished only by iframe_flag. <see cref="CreateAsync"/>
/// (Hosted Page) and <see cref="CreateIframeAsync"/> (embedded) both return a URL/token; Treps
/// then POSTs the result to your return_url (verify with <see cref="Hash.VerifyReturnUrlHash"/>).
/// <see cref="QueryAsync"/> lets you poll either kind of session's status by its token instead of
/// (or in addition to) relying on the return_url callback.
/// </summary>
public sealed class HostedPageResource
{
    private readonly TrepsClient _client;

    internal HostedPageResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/payment/hostedpage with iframe_flag: 0 — full-page redirect checkout.</summary>
    public Task<HostedPageInitResponseData> CreateAsync(HostedPageInitRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<HostedPageInitResponseData>(HttpMethod.Post, "/api/payment/hostedpage", request with { IframeFlag = 0 }, cancellationToken);

    /// <summary>
    /// POST /api/payment/hostedpage with iframe_flag: 1 — embed the returned Url in an
    /// <c>&lt;iframe src="..."&gt;</c> on <c>request.IframeWebUri</c> instead of redirecting the whole page.
    /// </summary>
    public Task<HostedPageInitResponseData> CreateIframeAsync(IFrameInitRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<HostedPageInitResponseData>(HttpMethod.Post, "/api/payment/hostedpage", request with { IframeFlag = 1 }, cancellationToken);

    /// <summary>GET /api/payment/hostedpage/{token} — works for both CreateAsync() and CreateIframeAsync() sessions.</summary>
    public Task<HostedPageQueryResponseData> QueryAsync(string token, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<HostedPageQueryResponseData>(HttpMethod.Get, $"/api/payment/hostedpage/{Uri.EscapeDataString(token)}", cancellationToken: cancellationToken);
}
