using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>POST /api/payment/pay3d — same body as <c>SaleRequest</c>, but ReturnUrl is required.</summary>
public record ThreeDSecureInitRequest : PaymentRequestBase
{
    [JsonPropertyName("transaction_type")]
    public int? TransactionType { get; init; }

    [JsonPropertyName("return_url")]
    public new required string ReturnUrl { get; init; }
}

public record ThreeDSecureInitResponseData
{
    /// <summary>Base64-encoded HTML auto-submit form that POSTs the customer's browser to the bank's 3DS page.</summary>
    [JsonPropertyName("redirect_content")]
    public required string RedirectContent { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }
}

/// <summary>POST /api/payment/pay3d/complete</summary>
public record ThreeDSecureCompleteRequest
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }
}

/// <summary>
/// Fields posted by the bank to the merchant's return_url after a 3D Secure (or Hosted Page)
/// flow completes. Verify the Hash field with <see cref="Hash.VerifyReturnUrlHash"/> before
/// trusting any of these values — never trust <c>ThreeDStatus == "SUCCESS"</c> alone.
/// </summary>
public record ReturnUrlPayload
{
    [JsonPropertyName("threeD_status")]
    public required string ThreeDStatus { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }

    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public string? ExternalTransactionId { get; init; }

    [JsonPropertyName("order_amount")]
    public required string OrderAmount { get; init; }

    [JsonPropertyName("amount")]
    public required string Amount { get; init; }

    [JsonPropertyName("card_amount")]
    public string? CardAmount { get; init; }

    [JsonPropertyName("interest_amount")]
    public string? InterestAmount { get; init; }

    [JsonPropertyName("point_amount")]
    public string? PointAmount { get; init; }

    [JsonPropertyName("wallet_amount")]
    public string? WalletAmount { get; init; }

    [JsonPropertyName("external_wallet_account_id")]
    public string? ExternalWalletAccountId { get; init; }

    [JsonPropertyName("wallet_payment_id")]
    public string? WalletPaymentId { get; init; }

    [JsonPropertyName("wallet_cashback")]
    public string? WalletCashback { get; init; }

    [JsonPropertyName("installment")]
    public required string Installment { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("complete_required")]
    public required string CompleteRequired { get; init; }

    [JsonPropertyName("duplicate_request")]
    public required string DuplicateRequest { get; init; }

    [JsonPropertyName("payment_status")]
    public required int PaymentStatus { get; init; }

    [JsonPropertyName("threeD_secure_type")]
    public required string ThreeDSecureType { get; init; }

    [JsonPropertyName("return_url")]
    public required string ReturnUrl { get; init; }

    [JsonPropertyName("retry_fail")]
    public string? RetryFail { get; init; }

    [JsonPropertyName("retry_count")]
    public string? RetryCount { get; init; }

    [JsonPropertyName("hash")]
    public required string Hash { get; init; }
}
