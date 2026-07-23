using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Insurance-sector payments. Per the docs site, only the initial payment (<see cref="PayAsync"/>)
/// has an insurance-specific request shape (a tokenized CardInsurance reference instead of raw
/// card details, plus mandatory IsMoto). Voiding or refunding an insurance payment uses the exact
/// same endpoints and request/response shapes as any other payment — use
/// <c>client.Payments.VoidAsync()</c> / <c>client.Payments.RefundAsync()</c> for those; there is
/// nothing insurance-specific to wrap. (The docs site's "detached refund" entry for insurance is,
/// as of this writing, byte-for-byte identical to its regular refund entry — likely a
/// documentation copy/paste rather than a distinct contract — so it isn't exposed as a separate
/// method here.)
/// </summary>
public sealed class InsuranceResource
{
    private readonly TrepsClient _client;

    internal InsuranceResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/payment/pay-insurance</summary>
    public Task<InsurancePaymentResponseData> PayAsync(InsurancePaymentRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<InsurancePaymentResponseData>(HttpMethod.Post, "/api/payment/pay-insurance", request, cancellationToken);
}
