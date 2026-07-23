using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Resources;

/// <summary>
/// Saved/tokenized card management: add, update, and remove. To search saved cards, use
/// <c>client.Query.CardsAsync()</c> instead — it's the same underlying <c>/api/card/search</c>
/// endpoint, grouped there since it's read-only.
/// </summary>
public sealed class CardResource
{
    private readonly TrepsClient _client;

    internal CardResource(TrepsClient client) => _client = client;

    /// <summary>POST /api/Card/add — tokenizes and saves a card for later use.</summary>
    public Task<AddCardResponseData> AddAsync(AddCardRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<AddCardResponseData>(HttpMethod.Post, "/api/Card/add", request, cancellationToken);

    /// <summary>
    /// POST /api/Card/update — updates the alias/owner name/expiry of a saved card. Note this
    /// endpoint returns a bare boolean, not an object, unlike every other write endpoint in this SDK.
    /// </summary>
    public Task<bool> UpdateAsync(UpdateCardRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<bool>(HttpMethod.Post, "/api/Card/update", request, cancellationToken);

    /// <summary>POST /api/Card/remove — deletes a saved card.</summary>
    public Task<RemoveCardResponseData> RemoveAsync(RemoveCardRequest request, CancellationToken cancellationToken = default) =>
        _client.RequestAsync<RemoveCardResponseData>(HttpMethod.Post, "/api/Card/remove", request, cancellationToken);
}
