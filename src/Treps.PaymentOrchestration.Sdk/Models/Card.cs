using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>POST /api/Card/add — tokenizes and saves a card for later use.</summary>
public record AddCardRequest
{
    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    [JsonPropertyName("customer_code")]
    public required string CustomerCode { get; init; }

    [JsonPropertyName("card_number")]
    public required string CardNumber { get; init; }

    /// <summary>MM/YYYY</summary>
    [JsonPropertyName("card_expire_date")]
    public required string CardExpireDate { get; init; }

    [JsonPropertyName("card_alias")]
    public required string CardAlias { get; init; }

    [JsonPropertyName("card_reference_code")]
    public required string CardReferenceCode { get; init; }
}

public record AddCardResponseData
{
    [JsonPropertyName("card_reference_code")]
    public required string CardReferenceCode { get; init; }

    [JsonPropertyName("card_token")]
    public required string CardToken { get; init; }

    [JsonPropertyName("card_alias")]
    public required string CardAlias { get; init; }

    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public required string CardLastFour { get; init; }

    /// <summary>MM/YYYY</summary>
    [JsonPropertyName("card_expire_date")]
    public required string CardExpireDate { get; init; }

    [JsonPropertyName("card_network")]
    public required string CardNetwork { get; init; }

    [JsonPropertyName("card_type")]
    public required string CardType { get; init; }

    [JsonPropertyName("card_brand")]
    public required string CardBrand { get; init; }

    [JsonPropertyName("bank_bic")]
    public required string BankBic { get; init; }

    [JsonPropertyName("virtual_card")]
    public required bool VirtualCard { get; init; }

    [JsonPropertyName("business_card")]
    public required bool BusinessCard { get; init; }

    [JsonPropertyName("card_country")]
    public required string CardCountry { get; init; }

    [JsonPropertyName("is_installment_supported")]
    public required bool IsInstallmentSupported { get; init; }
}

/// <summary>POST /api/Card/update — updates the alias/owner name/expiry of a previously saved card.</summary>
public record UpdateCardRequest
{
    [JsonPropertyName("customer_code")]
    public required string CustomerCode { get; init; }

    [JsonPropertyName("card_token")]
    public required string CardToken { get; init; }

    [JsonPropertyName("card_alias")]
    public required string CardAlias { get; init; }

    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    /// <summary>MM/YYYY</summary>
    [JsonPropertyName("card_expire_date")]
    public required string CardExpireDate { get; init; }
}

/// <summary>POST /api/Card/remove — deletes a previously saved card.</summary>
public record RemoveCardRequest
{
    [JsonPropertyName("customer_code")]
    public required string CustomerCode { get; init; }

    [JsonPropertyName("card_token")]
    public required string CardToken { get; init; }
}

public record RemoveCardResponseData
{
    [JsonPropertyName("removed")]
    public required bool Removed { get; init; }
}
