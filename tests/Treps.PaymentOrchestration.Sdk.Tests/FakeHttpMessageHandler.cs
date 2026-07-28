using System.Net;
using System.Text;

namespace Treps.PaymentOrchestration.Sdk.Tests;

/// <summary>Records each request and replays a queued response, mirroring the Node SDK's fetch-mock tests.</summary>
internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();

    public List<(HttpMethod Method, string Url, string? Authorization)> Requests { get; } = new();

    public List<string?> RequestBodies { get; } = new();

    public string? LastRequestBody => RequestBodies.Count > 0 ? RequestBodies[^1] : null;

    public void Enqueue(HttpStatusCode status, string body) =>
        _responses.Enqueue(new HttpResponseMessage(status) { Content = new StringContent(body, Encoding.UTF8, "application/json") });

    /// <summary>Queues a raw (non-JSON) response, e.g. a file download.</summary>
    public void EnqueueRaw(HttpStatusCode status, byte[] body, string contentType) =>
        _responses.Enqueue(new HttpResponseMessage(status) { Content = new ByteArrayContent(body) { Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType) } } });

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add((request.Method, request.RequestUri!.ToString(), request.Headers.Authorization?.ToString()));
        RequestBodies.Add(request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException("No more mocked HTTP responses queued.");
        }

        return _responses.Dequeue();
    }
}
