using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Asynchronous report export jobs (currently produced by
/// <c>client.Marketplace.Settlement.SummaryExportAsync()</c>). Search for a job's status,
/// download its finished file, or cancel it while pending/processing.
/// </summary>
public sealed class DownloadJobResource
{
    private readonly TrepsClient _client;

    internal DownloadJobResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/downloadjob/search — paginated, filterable by report type and/or job status.</summary>
    public Task<DownloadJobSearchResponseData> SearchAsync(DownloadJobSearchRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<DownloadJobSearchResponseData>(HttpMethod.Post, "/api/downloadjob/search", request ?? new DownloadJobSearchRequest(), cancellationToken);

    /// <summary>
    /// GET /api/downloadjob/{id}/download — downloads a completed (job_status = Completed) job's
    /// file. Returns the raw bytes plus whatever content-type/filename the server sent; unlike
    /// every other method in this SDK, this bypasses the <c>ApiResponse&lt;T&gt;</c> JSON envelope
    /// since the response body is the file itself.
    /// </summary>
    public Task<DownloadedFile> DownloadAsync(int id, CancellationToken cancellationToken = default) =>
        _client.DownloadFileAsync($"/api/downloadjob/{id}/download", cancellationToken);

    /// <summary>DELETE /api/downloadjob/{id}/cancel — cancels a pending or processing job.</summary>
    public Task<object?> CancelAsync(int id, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Delete, $"/api/downloadjob/{id}/cancel", cancellationToken: cancellationToken);
}
