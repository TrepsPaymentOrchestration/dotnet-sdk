using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Financial transaction operations: sale, pre-authorization, post-authorization (capture),
/// refund, and void. All map to <c>/api/payment/*</c>.
/// </summary>
public sealed class PaymentsResource
{
    private readonly TrepsClient _client;

    internal PaymentsResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/payment/pay with transaction_type: 1 — a direct, single-step charge.</summary>
    public Task<PaymentResponseData> SaleAsync(SaleRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PaymentResponseData>(HttpMethod.Post, "/api/payment/pay", request with { TransactionType = 1 }, cancellationToken);

    /// <summary>POST /api/payment/pay with transaction_type: 2 — reserves funds without capturing them.</summary>
    public Task<PaymentResponseData> PreAuthAsync(PreAuthRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PaymentResponseData>(HttpMethod.Post, "/api/payment/pay", request with { TransactionType = 2 }, cancellationToken);

    /// <summary>POST /api/payment/postauth — captures a previous PreAuth, fully or partially.</summary>
    public Task<PostAuthResponseData> PostAuthAsync(PostAuthRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PostAuthResponseData>(HttpMethod.Post, "/api/payment/postauth", request, cancellationToken);

    /// <summary>POST /api/payment/refund — refunds a previous sale/postAuth, fully or partially.</summary>
    public Task<RefundOrVoidResponseData> RefundAsync(RefundRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<RefundOrVoidResponseData>(HttpMethod.Post, "/api/payment/refund", request, cancellationToken);

    /// <summary>POST /api/payment/void — fully cancels a previous sale/preAuth.</summary>
    public Task<RefundOrVoidResponseData> VoidAsync(VoidRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<RefundOrVoidResponseData>(HttpMethod.Post, "/api/payment/void", request, cancellationToken);
}
