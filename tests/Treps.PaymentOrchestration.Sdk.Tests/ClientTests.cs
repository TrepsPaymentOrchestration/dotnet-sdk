using System.Net;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Xunit;

namespace Treps.PaymentOrchestration.Sdk.Tests;

public class ClientTests
{
    private static (TrepsClient Client, FakeHttpMessageHandler Handler) MakeClient(
        TrepsEnvironment environment = TrepsEnvironment.Sandbox,
        string? baseUrl = null)
    {
        var handler = new FakeHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var client = new TrepsClient(
            username: "user",
            password: "pass",
            merchantId: 1,
            environment: environment,
            baseUrl: baseUrl,
            httpClient: httpClient);

        return (client, handler);
    }

    private static string LoginBody(string accessToken, long expireInMs) =>
        $$"""
        {"status":true,"message":null,"data":{"access_token":"{{accessToken}}","expire_in":{{expireInMs}},"scheme":"Bearer","token_policy":"apiuser"},"errors":null}
        """;

    private static long NowMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    [Fact]
    public async Task LogsInOnceAndReusesTheCachedTokenAcrossMultipleRequests()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody("token-1", NowMs() + 60_000));
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":{"ok":1},"errors":null}""");
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":{"ok":2},"errors":null}""");

        await client.RequestAsync<object>(HttpMethod.Get, "/api/whatever");
        await client.RequestAsync<object>(HttpMethod.Get, "/api/whatever");

        Assert.Equal(3, handler.Requests.Count); // 1 login + 2 requests
        Assert.Equal("https://poapi.treps.tr/api/auth", handler.Requests[0].Url);
        Assert.Equal("Bearer token-1", handler.Requests[1].Authorization);
    }

    [Fact]
    public async Task ReAuthenticatesOnceTheCachedTokenHasExpired()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody("token-1", NowMs() - 1_000));
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":{"ok":1},"errors":null}""");
        handler.Enqueue(HttpStatusCode.OK, LoginBody("token-2", NowMs() + 60_000));
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":{"ok":2},"errors":null}""");

        await client.RequestAsync<object>(HttpMethod.Get, "/api/whatever");
        await client.RequestAsync<object>(HttpMethod.Get, "/api/whatever");

        Assert.Equal(4, handler.Requests.Count); // login, request, re-login, request
        Assert.Equal("Bearer token-2", handler.Requests[3].Authorization);
    }

    [Fact]
    public async Task ThrowsTrepsApiExceptionWhenTheApiReturnsStatusFalse()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody("token-1", NowMs() + 60_000));
        handler.Enqueue(HttpStatusCode.OK, """{"status":false,"message":"insufficient_balance","data":null,"errors":["insufficient_balance"]}""");

        await Assert.ThrowsAsync<TrepsApiException>(
            () => client.RequestAsync<object>(HttpMethod.Post, "/api/payment/pay", new { }));
    }

    [Fact]
    public async Task ThrowsTrepsApiExceptionOnNon2xxHttpResponses()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(
            HttpStatusCode.Unauthorized,
            """{"status":false,"message":"invalid_username_or_password","data":null,"errors":["invalid_username_or_password"]}""");

        await Assert.ThrowsAsync<TrepsApiException>(() => client.RequestAsync<object>(HttpMethod.Get, "/api/whatever"));
    }

    [Fact]
    public async Task UsesTheProductionBaseUrlWhenEnvironmentIsProduction()
    {
        var (client, handler) = MakeClient(environment: TrepsEnvironment.Production);
        handler.Enqueue(HttpStatusCode.OK, LoginBody("token-1", NowMs() + 60_000));
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":{},"errors":null}""");

        await client.RequestAsync<object>(HttpMethod.Get, "/api/whatever");

        Assert.Equal("https://api.treps.io/api/auth", handler.Requests[0].Url);
    }

    [Fact]
    public async Task RespectsACustomBaseUrlOverride()
    {
        var (client, handler) = MakeClient(baseUrl: "https://custom.example.com");
        handler.Enqueue(HttpStatusCode.OK, LoginBody("token-1", NowMs() + 60_000));
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":{},"errors":null}""");

        await client.RequestAsync<object>(HttpMethod.Get, "/api/whatever");

        Assert.Equal("https://custom.example.com/api/auth", handler.Requests[0].Url);
    }
}
