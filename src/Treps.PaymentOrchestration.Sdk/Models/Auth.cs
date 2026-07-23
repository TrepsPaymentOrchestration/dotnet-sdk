using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

internal record LoginRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }

    [JsonPropertyName("merchantId")]
    public required int MerchantId { get; init; }
}

public record LoginResponseData
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    /// <summary>Absolute expiry instant, epoch milliseconds (NOT a duration/TTL).</summary>
    [JsonPropertyName("expire_in")]
    public required long ExpireIn { get; init; }

    [JsonPropertyName("scheme")]
    public required string Scheme { get; init; }

    [JsonPropertyName("token_policy")]
    public required string TokenPolicy { get; init; }
}
