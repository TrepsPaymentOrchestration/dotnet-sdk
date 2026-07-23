using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>
/// Tokenized card reference for an insurance payment — no raw card number/expiry/CVV, since the
/// card was already processed elsewhere. Distinct from <see cref="Card"/>, used by Sale/PreAuth.
/// </summary>
public record InsuranceCard
{
    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    /// <summary>First 6-8 digits of the card.</summary>
    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public required string CardLastFour { get; init; }

    /// <summary>Card owner's tax ID (VKN) or citizenship number (TCKN) — required for insurance payments.</summary>
    [JsonPropertyName("owner_vkn_tckn")]
    public required string OwnerVknTckn { get; init; }

    [JsonPropertyName("card_owner_customer_id")]
    public required string CardOwnerCustomerId { get; init; }

    [JsonPropertyName("card_alias")]
    public string? CardAlias { get; init; }

    [JsonPropertyName("card_reference_code")]
    public string? CardReferenceCode { get; init; }
}

/// <summary>
/// POST /api/payment/pay-insurance — identical to PaymentsResource.SaleAsync()/PreAuthAsync()
/// except the card is a tokenized CardInsurance reference (not raw card details), and IsMoto is
/// mandatory (must be true) rather than optional.
/// </summary>
public record InsurancePaymentRequest
{
    /// <summary>ManuelPos=1, LinkPayment=2, API=3, Hostedpage=4, IFrame=4.</summary>
    [JsonPropertyName("payment_request_type")]
    public int? PaymentRequestType { get; init; }

    /// <summary>Auth=1, PreAuth=2.</summary>
    [JsonPropertyName("transaction_type")]
    public int? TransactionType { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public string? ExternalTransactionId { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("split_payment")]
    public bool? SplitPayment { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; init; }

    [JsonPropertyName("installment")]
    public required int Installment { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("client_ip")]
    public required string ClientIp { get; init; }

    [JsonPropertyName("userId")]
    public int? UserId { get; init; }

    [JsonPropertyName("vpos_code")]
    public string? VposCode { get; init; }

    [JsonPropertyName("retry_fail")]
    public bool? RetryFail { get; init; }

    [JsonPropertyName("customer_commission_plan_code")]
    public string? CustomerCommissionPlanCode { get; init; }

    /// <summary>Mandatory for insurance transactions — must be true.</summary>
    [JsonPropertyName("is_moto")]
    public required bool IsMoto { get; init; }

    [JsonPropertyName("card_insurance")]
    public required InsuranceCard CardInsurance { get; init; }

    [JsonPropertyName("buyer")]
    public Buyer? Buyer { get; init; }

    [JsonPropertyName("products")]
    public List<Product>? Products { get; init; }

    [JsonPropertyName("billing_address")]
    public Address? BillingAddress { get; init; }

    [JsonPropertyName("shipping_address")]
    public Address? ShippingAddress { get; init; }

    [JsonPropertyName("sub_merchants")]
    public List<SubMerchant>? SubMerchants { get; init; }
}

public record InsurancePaymentResponseData : PaymentResponseData
{
    /// <summary>
    /// Not present in the documented example response for this endpoint, but present on the
    /// equivalent field set for regular Sale responses — included as optional since it's the
    /// clearest actual "this was an insurance payment" marker found in the source docs. Verify
    /// against a live response before relying on it.
    /// </summary>
    [JsonPropertyName("insurance_payment_flag")]
    public int? InsurancePaymentFlag { get; init; }

    /// <summary>See caveat on <see cref="InsurancePaymentFlag"/> above.</summary>
    [JsonPropertyName("insurance_card_vkn_tckn")]
    public string? InsuranceCardVknTckn { get; init; }
}
