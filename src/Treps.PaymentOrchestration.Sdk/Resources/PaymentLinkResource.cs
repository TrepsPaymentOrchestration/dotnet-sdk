using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Shareable payment links (email/SMS/WhatsApp) — the customer completes payment on a Treps-
/// hosted page reached via the link, no card details ever touch your servers.
/// </summary>
public sealed class PaymentLinkResource
{
    private readonly TrepsClient _client;

    internal PaymentLinkResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/payment/createpaylink</summary>
    public Task<PaymentLinkCreateResponseData> CreateAsync(PaymentLinkCreateRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PaymentLinkCreateResponseData>(HttpMethod.Post, "/api/payment/createpaylink", request, cancellationToken);

    /// <summary>POST /api/payment/getpaylink — full details + payment status for a link, by its token.</summary>
    public Task<PaymentLinkQueryResponseData> GetAsync(PaymentLinkQueryRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PaymentLinkQueryResponseData>(HttpMethod.Post, "/api/payment/getpaylink", request, cancellationToken);

    /// <summary>POST /api/payment/listpaylinks — paginated, filterable list of payment links.</summary>
    public Task<PaymentLinkListResponseData> ListAsync(PaymentLinkListRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PaymentLinkListResponseData>(HttpMethod.Post, "/api/payment/listpaylinks", request ?? new PaymentLinkListRequest(), cancellationToken);
}
