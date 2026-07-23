using System.Net;
using System.Text;

namespace Treps.PaymentOrchestration.Sdk.Tests;

/// <summary>Records each request and replays a queued response, mirroring the Node SDK's fetch-mock tests.</summary>
internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<(HttpStatusCode Status, string Body)> _responses = new();

    public List<(HttpMethod Method, string Url, string? Authorization)> Requests { get; } = new();

    public void Enqueue(HttpStatusCode status, string body) => _responses.Enqueue((status, body));

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add((request.Method, request.RequestUri!.ToString(), request.Headers.Authorization?.ToString()));

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException("No more mocked HTTP responses queued.");
        }

        var (status, body) = _responses.Dequeue();
        var response = new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };

        return Task.FromResult(response);
    }
}
