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
/// A single background export job, as returned by POST /api/downloadjob/search. Confirmed
/// against a live response captured 2026-07-29.
/// </summary>
public record DownloadJob
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>1 = SubMerchantSettlement.</summary>
    [JsonPropertyName("report_type")]
    public required int ReportType { get; init; }

    [JsonPropertyName("report_name")]
    public string? ReportName { get; init; }

    /// <summary>0 = Pending, 1 = Processing, 2 = Completed, 3 = Failed, 4 = Cancelling, 5 = Cancelled.</summary>
    [JsonPropertyName("job_status")]
    public required int JobStatus { get; init; }

    [JsonPropertyName("total_row_count")]
    public required int TotalRowCount { get; init; }

    [JsonPropertyName("processed_row_count")]
    public required int ProcessedRowCount { get; init; }

    [JsonPropertyName("progress_percentage")]
    public required double ProgressPercentage { get; init; }

    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; init; }

    /// <summary>ISO datetime string.</summary>
    [JsonPropertyName("insert_date")]
    public required string InsertDate { get; init; }

    /// <summary>ISO datetime string, null until the job starts processing.</summary>
    [JsonPropertyName("started_date")]
    public string? StartedDate { get; init; }

    /// <summary>ISO datetime string, null until the job completes.</summary>
    [JsonPropertyName("completed_date")]
    public string? CompletedDate { get; init; }

    /// <summary>ISO datetime string, null unless the job was cancelled.</summary>
    [JsonPropertyName("cancelled_date")]
    public string? CancelledDate { get; init; }
}

/// <summary>
/// Response envelope assumed to mirror the pagination shape used consistently elsewhere in the
/// marketplace API (see <see cref="SubMerchantFindResponseData"/>, <see cref="SettlementSummaryResponseData"/>) —
/// the <see cref="Items"/> shape itself is confirmed live (see <see cref="DownloadJob"/>), but the
/// surrounding total_count/page/page_size envelope is not independently confirmed for this
/// specific endpoint as of this writing.
/// </summary>
public record DownloadJobSearchResponseData
{
    [JsonPropertyName("items")]
    public required List<DownloadJob> Items { get; init; }

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
