using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>Marketplace hakediş (settlement) reporting: summary, exported summary, and line-item detail.</summary>
public sealed class SettlementResource
{
    private readonly TrepsClient _client;

    internal SettlementResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/marketplace/settlement/summary — paginated per-sub-merchant balances.</summary>
    public Task<SettlementSummaryResponseData> SummaryAsync(SettlementSummaryRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<SettlementSummaryResponseData>(HttpMethod.Post, "/api/marketplace/settlement/summary", request ?? new SettlementSummaryRequest(), cancellationToken);

    /// <summary>
    /// POST /api/marketplace/settlement/summary/export — asynchronously generates a downloadable
    /// export of <see cref="SummaryAsync"/>'s data. Track and fetch it via <c>client.DownloadJobs</c>
    /// (filter by <c>report_type</c> = SubMerchantSettlement). Response shape isn't documented by
    /// the API spec this SDK was built from.
    /// </summary>
    public Task<object?> SummaryExportAsync(SettlementSummaryExportRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/settlement/summary/export", request, cancellationToken);

    /// <summary>POST /api/marketplace/settlement/detail — paginated, filterable order-level settlement line items.</summary>
    public Task<SettlementDetailResponseData> DetailAsync(SettlementDetailRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<SettlementDetailResponseData>(HttpMethod.Post, "/api/marketplace/settlement/detail", request ?? new SettlementDetailRequest(), cancellationToken);
}
