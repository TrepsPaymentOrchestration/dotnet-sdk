using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// 3D Secure operations. <see cref="InitAsync"/> starts the challenge (returns an HTML form to
/// redirect the customer's browser to their bank); after the bank redirects back to your
/// return_url (verify with <see cref="Hash.VerifyReturnUrlHash"/>), call <see cref="CompleteAsync"/>
/// to finalize the charge.
/// </summary>
public sealed class ThreeDSecureResource
{
    private readonly TrepsClient _client;

    internal ThreeDSecureResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/payment/pay3d</summary>
    public Task<ThreeDSecureInitResponseData> InitAsync(ThreeDSecureInitRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<ThreeDSecureInitResponseData>(HttpMethod.Post, "/api/payment/pay3d", request, cancellationToken);

    /// <summary>POST /api/payment/pay3d/complete</summary>
    public Task<PaymentResponseData> CompleteAsync(ThreeDSecureCompleteRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<PaymentResponseData>(HttpMethod.Post, "/api/payment/pay3d/complete", request, cancellationToken);
}
