using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>POST /api/downloadjob/search</summary>
public record DownloadJobSearchRequest
{
    /// <summary>1 = SubMerchantSettlement.</summary>
    [JsonPropertyName("report_type")]
    public int? ReportType { get; init; }

    /// <summary>0 = Pending, 1 = Processing, 2 = Completed, 3 = Failed, 4 = Cancelling, 5 = Cancelled.</summary>
    [JsonPropertyName("job_status")]
    public int? JobStatus { get; init; }

    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }
}

/// <summary>
/// A single background export job. Only <see cref="Id"/>, <see cref="ReportType"/>, and
/// <see cref="JobStatus"/> are documented by the API spec this SDK was built from — they mirror
/// the request filter names in <see cref="DownloadJobSearchRequest"/>, per this SDK's usual
/// filter/field name symmetry (e.g. <c>TransactionReportQueryRequest.TransactionStatus</c> vs.
/// <c>TransactionReportItem</c>'s field of the same name). The live response may carry additional
/// fields (file name, created date, etc.) not modeled here; verify and extend as needed.
/// </summary>
public record DownloadJobItem
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("report_type")]
    public required int ReportType { get; init; }

    [JsonPropertyName("job_status")]
    public required int JobStatus { get; init; }
}

/// <summary>
/// Response envelope assumed to mirror the pagination shape used consistently elsewhere in the
/// marketplace API (see <see cref="SubMerchantFindResponseData"/>, <see cref="SettlementSummaryResponseData"/>) —
/// not independently confirmed for this specific endpoint as of this writing.
/// </summary>
public record DownloadJobSearchResponseData
{
    [JsonPropertyName("items")]
    public required List<DownloadJobItem> Items { get; init; }

    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("page")]
    public required int Page { get; init; }

    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }
}

/// <summary>
/// The raw bytes of a downloaded report file (GET /api/downloadjob/{id}/download), plus whatever
/// content-type/filename metadata the server sent. This endpoint returns a binary file, not the
/// usual <c>ApiResponse&lt;T&gt;</c> JSON envelope, so it bypasses <see cref="TrepsClient.RequestAsync{T}"/>
/// — see <see cref="TrepsClient.DownloadFileAsync"/>.
/// </summary>
public sealed record DownloadedFile(byte[] Content, string? ContentType, string? FileName);
