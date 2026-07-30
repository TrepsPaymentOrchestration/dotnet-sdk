using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>Marketplace order operations: approve, pay, seize, refund, collect debt, and cancel.</summary>
public sealed class MarketplaceOrderResource
{
    private readonly TrepsClient _client;

    internal MarketplaceOrderResource(TrepsClient client) => _client = client;

    /// <summary>
    /// POST /api/marketplace/order/approve — batch endpoint; the request body is the array
    /// directly. Response shape isn't documented by the API spec this SDK was built from.
    /// </summary>
    public Task<object?> ApproveAsync(List<OrderApproveItem> items, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/order/approve", items, cancellationToken);

    /// <summary>
    /// POST /api/marketplace/order/pay — batch endpoint; the request body is the array directly.
    /// Response shape isn't documented by the API spec this SDK was built from.
    /// </summary>
    public Task<object?> PayAsync(List<OrderPayItem> items, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/order/pay", items, cancellationToken);

    /// <summary>
    /// POST /api/marketplace/order/pay/allocate — batch endpoint; the request body is the array
    /// directly. <b>Atomic:</b> see the caveat on <see cref="OrderPayAllocateResponseData"/> — a
    /// top-level <c>Success == false</c> means nothing in the batch was actually applied,
    /// regardless of individual item success flags.
    /// </summary>
    public Task<OrderPayAllocateResponseData> AllocatePayAsync(List<OrderPayAllocateItem> items, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<OrderPayAllocateResponseData>(HttpMethod.Post, "/api/marketplace/order/pay/allocate", items, cancellationToken);

    /// <summary>POST /api/marketplace/order/seized — marks (part of) an order's funds as seized/held.</summary>
    public Task<object?> SeizedAsync(OrderSeizedRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/order/seized", request, cancellationToken);

    /// <summary>
    /// POST /api/marketplace/order/refund — refunds a sub-merchant's allocation of a marketplace
    /// order. Distinct from <c>client.Payments.RefundAsync()</c> (POST /api/payment/refund),
    /// which refunds the top-level payment and can itself carry a <c>sub_merchants</c> allocation
    /// list (see <see cref="RefundRequest.SubMerchants"/>). This is a valid endpoint, but the
    /// reference frontend (MerchantPanel) does not use it — it refunds marketplace orders through
    /// <c>client.Payments.RefundAsync()</c> with <c>SubMerchants</c> instead; prefer that path
    /// unless you specifically need order-level, per-sub-merchant refund semantics.
    /// </summary>
    public Task<object?> RefundAsync(OrderRefundRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/order/refund", request, cancellationToken);

    /// <summary>POST /api/marketplace/order/collectdept — collects an outstanding debt against a sub-merchant.</summary>
    public Task<object?> CollectDebtAsync(OrderCollectDebtRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/order/collectdept", request, cancellationToken);

    /// <summary>
    /// POST /api/marketplace/order/cancel — cancels/voids a sub-merchant's allocation of a
    /// marketplace order. Like <see cref="RefundAsync"/>, this is valid but not what the
    /// reference frontend (MerchantPanel) uses; it voids marketplace orders through
    /// <c>client.Payments.VoidAsync()</c> with <c>SubMerchants</c> (see
    /// <see cref="VoidRequest.SubMerchants"/>) instead — prefer that path unless you specifically
    /// need order-level, per-sub-merchant cancellation.
    /// </summary>
    public Task<object?> CancelAsync(OrderCancelRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/order/cancel", request, cancellationToken);
}
