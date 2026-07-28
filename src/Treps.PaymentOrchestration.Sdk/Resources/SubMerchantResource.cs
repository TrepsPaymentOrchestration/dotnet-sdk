using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>Marketplace sub-merchant (alt üye işyeri) management: add/update/get/find/import.</summary>
public sealed class SubMerchantResource
{
    private readonly TrepsClient _client;

    internal SubMerchantResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/marketplace/submerchant/add</summary>
    public Task<SubMerchantResponseData> AddAsync(SubMerchantAddRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<SubMerchantResponseData>(HttpMethod.Post, "/api/marketplace/submerchant/add", request, cancellationToken);

    /// <summary>POST /api/marketplace/submerchant/update</summary>
    public Task<SubMerchantResponseData> UpdateAsync(SubMerchantUpdateRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<SubMerchantResponseData>(HttpMethod.Post, "/api/marketplace/submerchant/update", request, cancellationToken);

    /// <summary>POST /api/marketplace/submerchant/get</summary>
    public Task<SubMerchantResponseData> GetAsync(SubMerchantGetRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<SubMerchantResponseData>(HttpMethod.Post, "/api/marketplace/submerchant/get", request, cancellationToken);

    /// <summary>POST /api/marketplace/submerchant/find — paginated, filterable sub-merchant search.</summary>
    public Task<SubMerchantFindResponseData> FindAsync(SubMerchantFindRequest? request = null, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<SubMerchantFindResponseData>(HttpMethod.Post, "/api/marketplace/submerchant/find", request ?? new SubMerchantFindRequest(), cancellationToken);

    /// <summary>
    /// POST /api/marketplace/submerchant/import — bulk-creates sub-merchants from a list of
    /// <see cref="SubMerchantAddRequest"/> objects (the request body is the array directly, not
    /// wrapped in an object). Response shape isn't documented by the API spec this SDK was built
    /// from; assumed to mirror <see cref="AddAsync"/>'s response, one entry per submitted item —
    /// verify against a live response.
    /// </summary>
    public Task<List<SubMerchantResponseData>> ImportAsync(List<SubMerchantAddRequest> subMerchants, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<List<SubMerchantResponseData>>(HttpMethod.Post, "/api/marketplace/submerchant/import", subMerchants, cancellationToken);
}
