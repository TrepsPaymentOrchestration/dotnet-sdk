using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Read-only lookup/reporting operations: BIN lookups, transaction/order detail and reporting,
/// customer commission schemes, installment options, and saved-card search.
/// </summary>
public sealed class QueryResource
{
    private readonly TrepsClient _client;

    internal QueryResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/references/bin — looks up card brand/network/country from its BIN.</summary>
    public Task<BinQueryResponseData> BinAsync(BinQueryRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<BinQueryResponseData>(HttpMethod.Post, "/api/references/bin", request, cancellationToken);

    /// <summary>POST /api/report/transaction-detail</summary>
    public Task<TransactionDetailQueryResponseData> TransactionDetailAsync(TransactionDetailQueryRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<TransactionDetailQueryResponseData>(HttpMethod.Post, "/api/report/transaction-detail", request, cancellationToken);

    /// <summary>
    /// POST /api/report/transaction-report — paginated, filterable transaction list. Returns a
    /// bare array — unlike <see cref="OrderReportAsync"/>, which wraps in {TotalCount, Data}.
    /// </summary>
    public Task<List<TransactionReportItem>> TransactionReportAsync(TransactionReportQueryRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<List<TransactionReportItem>>(HttpMethod.Post, "/api/report/transaction-report", request ?? new TransactionReportQueryRequest(), cancellationToken);

    /// <summary>POST /api/report/order-detail — requires ExternalOrderId or OrderId.</summary>
    public Task<OrderDetailQueryResponseData> OrderDetailAsync(OrderDetailQueryRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<OrderDetailQueryResponseData>(HttpMethod.Post, "/api/report/order-detail", request, cancellationToken);

    /// <summary>POST /api/report/order-report — paginated, filterable order list.</summary>
    public Task<OrderReportQueryResponseData> OrderReportAsync(OrderReportQueryRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<OrderReportQueryResponseData>(HttpMethod.Post, "/api/report/order-report", request ?? new OrderReportQueryRequest(), cancellationToken);

    /// <summary>GET /api/commission/customercommission — lists your customer commission schemes.</summary>
    public Task<List<CustomerCommissionScheme>> CustomerCommissionsAsync(CancellationToken cancellationToken = default) =>
        _client.RequestAsync<List<CustomerCommissionScheme>>(HttpMethod.Get, "/api/commission/customercommission", cancellationToken: cancellationToken);

    /// <summary>
    /// GET /api/commission/customercommissionitemcode/{code} — per-card-network commission
    /// rates for a scheme returned by <see cref="CustomerCommissionsAsync"/>.
    /// </summary>
    public Task<CustomerCommissionItemQueryResponseData> CustomerCommissionItemsAsync(string code, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<CustomerCommissionItemQueryResponseData>(HttpMethod.Get, $"/api/commission/customercommissionitemcode/{Uri.EscapeDataString(code)}", cancellationToken: cancellationToken);

    /// <summary>POST /api/commission/getinstallmentinfo — available installment options for a card/plan.</summary>
    public Task<InstallmentQueryResponseData> InstallmentsAsync(InstallmentQueryRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<InstallmentQueryResponseData>(HttpMethod.Post, "/api/commission/getinstallmentinfo", request, cancellationToken);

    /// <summary>POST /api/card/search — search saved/tokenized cards.</summary>
    public Task<CardQueryResponseData> CardsAsync(CardQueryRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<CardQueryResponseData>(HttpMethod.Post, "/api/card/search", request, cancellationToken);
}
