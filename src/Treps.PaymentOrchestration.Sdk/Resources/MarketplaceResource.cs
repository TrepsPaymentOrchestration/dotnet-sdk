using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Marketplace / Split Payment: sub-merchant management (<see cref="SubMerchants"/>), order
/// operations (<see cref="Order"/>), settlement reporting (<see cref="Settlement"/>), and the
/// write-only platform setting exposed directly as <see cref="ConfigAsync"/>.
/// </summary>
public sealed class MarketplaceResource
{
    private readonly TrepsClient _client;

    internal MarketplaceResource(TrepsClient client)
    {
        _client = client;
        SubMerchants = new SubMerchantResource(client);
        Order = new MarketplaceOrderResource(client);
        Settlement = new SettlementResource(client);
    }

    /// <summary>Add/update/get/find/import sub-merchants (alt üye işyeri).</summary>
    public SubMerchantResource SubMerchants { get; }

    /// <summary>Approve, pay, seize, refund, collect debt against, and cancel marketplace orders.</summary>
    public MarketplaceOrderResource Order { get; }

    /// <summary>Hakediş (settlement) summary and detail reporting.</summary>
    public SettlementResource Settlement { get; }

    /// <summary>
    /// POST /api/marketplace/config — sets platform-wide marketplace settings. Write-only: there
    /// is no GET/read endpoint for this on the backend as of this writing, so there's no way to
    /// fetch the current value back (through this SDK or the API itself).
    /// </summary>
    public Task<object?> ConfigAsync(MarketplaceConfigRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<object?>(HttpMethod.Post, "/api/marketplace/config", request, cancellationToken);
}
