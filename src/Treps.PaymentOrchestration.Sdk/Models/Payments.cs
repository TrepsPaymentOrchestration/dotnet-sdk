using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>POST /api/payment/pay — transaction_type: 1 (Auth). TransactionType is set automatically by <c>PaymentsResource.SaleAsync()</c>.</summary>
public record SaleRequest : PaymentRequestBase
{
    [JsonPropertyName("transaction_type")]
    public int TransactionType { get; init; } = 1;
}

/// <summary>POST /api/payment/pay — transaction_type: 2 (PreAuth). TransactionType is set automatically by <c>PaymentsResource.PreAuthAsync()</c>.</summary>
public record PreAuthRequest : PaymentRequestBase
{
    [JsonPropertyName("transaction_type")]
    public int TransactionType { get; init; } = 2;
}

/// <summary>POST /api/payment/postauth</summary>
public record PostAuthRequest
{
    /// <summary>Payment ID of the original sale/preAuth. Required if TransactionId is omitted.</summary>
    [JsonPropertyName("payment_id")]
    public string? PaymentId { get; init; }

    /// <summary>Transaction ID of the original sale/preAuth. Required if PaymentId is omitted.</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    /// <summary>Amount to capture. Omit to capture the full original PreAuth amount.</summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }

    [JsonPropertyName("clientIp")]
    public required string ClientIp { get; init; }
}

/// <summary>
/// Per-sub-merchant refund allocation for a Marketplace (split-payment) order, used by
/// <see cref="RefundRequest.SubMerchants"/> and <see cref="VoidRequest.SubMerchants"/>.
///
/// <para><b>Naming note:</b> the wire field really is <c>reference_id</c> here — not
/// <c>sub_merchant_reference_id</c> like every marketplace settlement/order response uses for
/// the same concept. This asymmetry is on the backend's request side, confirmed against the
/// live contract, not a typo in this SDK.</para>
/// </summary>
public record RefundSubMerchantAllocation
{
    [JsonPropertyName("reference_id")]
    public required string ReferenceId { get; init; }

    [JsonPropertyName("refund_amount")]
    public required decimal RefundAmount { get; init; }
}

/// <summary>POST /api/payment/refund</summary>
public record RefundRequest
{
    /// <summary>Payment ID of the original sale/postAuth. Required if TransactionId is omitted.</summary>
    [JsonPropertyName("payment_id")]
    public string? PaymentId { get; init; }

    /// <summary>Transaction ID of the original sale/postAuth. Required if PaymentId is omitted.</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    /// <summary>Amount to refund. Omit to refund the full original amount.</summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }

    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    [JsonPropertyName("clientIp")]
    public required string ClientIp { get; init; }

    /// <summary>
    /// Marketplace (split-payment) orders only — how the refund is allocated across
    /// sub-merchants. Omit to refund the full amount as a single, non-split transaction. See
    /// <see cref="RefundSubMerchantAllocation"/> for a field-naming caveat.
    /// </summary>
    [JsonPropertyName("sub_merchants")]
    public List<RefundSubMerchantAllocation>? SubMerchants { get; init; }
}

/// <summary>POST /api/payment/void</summary>
public record VoidRequest
{
    /// <summary>Payment ID of the original sale/preAuth. Required if TransactionId is omitted.</summary>
    [JsonPropertyName("payment_id")]
    public string? PaymentId { get; init; }

    /// <summary>Transaction ID of the original sale/preAuth. Required if PaymentId is omitted.</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    [JsonPropertyName("clientIp")]
    public required string ClientIp { get; init; }

    /// <summary>
    /// Marketplace (split-payment) orders only — how the voided refund is allocated across
    /// sub-merchants. Omit to void the full amount as a single, non-split transaction. See
    /// <see cref="RefundSubMerchantAllocation"/> for a field-naming caveat.
    /// </summary>
    [JsonPropertyName("sub_merchants")]
    public List<RefundSubMerchantAllocation>? SubMerchants { get; init; }
}

public record PostAuthResponseData
{
    [JsonPropertyName("status")]
    public required bool Status { get; init; }

    [JsonPropertyName("vpos_payment_id")]
    public required string VposPaymentId { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("preauth_amount")]
    public required decimal PreauthAmount { get; init; }

    [JsonPropertyName("total_postauth_amount")]
    public required decimal TotalPostauthAmount { get; init; }

    [JsonPropertyName("transaction_status")]
    public required int TransactionStatus { get; init; }

    [JsonPropertyName("transaction_status_code")]
    public required string TransactionStatusCode { get; init; }

    [JsonPropertyName("bank_reference_id")]
    public string? BankReferenceId { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public string? BankBatchNumber { get; init; }

    [JsonPropertyName("result_code")]
    public required string ResultCode { get; init; }

    [JsonPropertyName("result_message")]
    public string? ResultMessage { get; init; }

    [JsonPropertyName("parent_transaction_sub_id")]
    public required int ParentTransactionSubId { get; init; }

    [JsonPropertyName("created_user_id")]
    public required int CreatedUserId { get; init; }
}

public record RefundOrVoidResponseData
{
    [JsonPropertyName("vpos_payment_id")]
    public required string VposPaymentId { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("transaction_status")]
    public required int TransactionStatus { get; init; }

    [JsonPropertyName("transaction_status_code")]
    public required string TransactionStatusCode { get; init; }

    [JsonPropertyName("bank_reference_id")]
    public string? BankReferenceId { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public string? BankBatchNumber { get; init; }

    [JsonPropertyName("result_code")]
    public required string ResultCode { get; init; }

    [JsonPropertyName("result_message")]
    public string? ResultMessage { get; init; }

    [JsonPropertyName("parent_transaction_sub_id")]
    public required int ParentTransactionSubId { get; init; }

    [JsonPropertyName("created_user_id")]
    public required int CreatedUserId { get; init; }
}
