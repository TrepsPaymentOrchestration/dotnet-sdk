using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>Standard envelope returned by every Treps API endpoint.</summary>
public sealed class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public bool Status { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("data")]
    public T? Data { get; init; }

    [JsonPropertyName("errors")]
    public List<string>? Errors { get; init; }
}

public record Card
{
    /// <summary>Token of a previously saved card. When set, CardNumber/Cvv/etc. may be omitted.</summary>
    [JsonPropertyName("hosted_card_token")]
    public string? HostedCardToken { get; init; }

    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    [JsonPropertyName("card_number")]
    public required string CardNumber { get; init; }

    [JsonPropertyName("card_expire_year")]
    public required string CardExpireYear { get; init; }

    [JsonPropertyName("card_expire_month")]
    public required string CardExpireMonth { get; init; }

    [JsonPropertyName("card_cvv")]
    public required string CardCvv { get; init; }

    [JsonPropertyName("card_owner_customer_id")]
    public required string CardOwnerCustomerId { get; init; }

    [JsonPropertyName("card_alias")]
    public string? CardAlias { get; init; }

    [JsonPropertyName("card_reference_code")]
    public string? CardReferenceCode { get; init; }

    /// <summary>1 to tokenize the card for future use.</summary>
    [JsonPropertyName("save_card")]
    public int? SaveCard { get; init; }
}

public record Buyer
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("status")]
    public int? Status { get; init; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("surname")]
    public string? Surname { get; init; }

    [JsonPropertyName("citizenship_number")]
    public string? CitizenshipNumber { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("zip_code")]
    public string? ZipCode { get; init; }
}

public record Address
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("address")]
    public string? AddressLine { get; init; }

    [JsonPropertyName("zip_code")]
    public string? ZipCode { get; init; }
}

public record Product
{
    [JsonPropertyName("product_id")]
    public string? ProductId { get; init; }

    [JsonPropertyName("category")]
    public string? Category { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("price")]
    public decimal? Price { get; init; }

    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

public record SubMerchant
{
    [JsonPropertyName("reference_id")]
    public required string ReferenceId { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }
}

/// <summary>
/// Shared body fields for <c>Sale</c>, <c>PreAuth</c>, and 3D Secure <c>Init</c> — all three post
/// to an /api/payment/pay* endpoint with this same shape.
/// </summary>
public record PaymentRequestBase
{
    /// <summary>ManuelPos=1, LinkPayment=2, API=3, Hostedpage=4, IFrame=4.</summary>
    [JsonPropertyName("payment_request_type")]
    public int? PaymentRequestType { get; init; }

    /// <summary>Unique id for this transaction, up to 50 chars.</summary>
    [JsonPropertyName("external_transaction_id")]
    public string? ExternalTransactionId { get; init; }

    /// <summary>Unique id for the order, up to 50 chars.</summary>
    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("split_payment")]
    public bool? SplitPayment { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    /// <summary>ISO currency code, e.g. TRY, USD, EUR, GBP.</summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; init; }

    /// <summary>Number of installments. 1 = single payment.</summary>
    [JsonPropertyName("installment")]
    public required int Installment { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("client_ip")]
    public required string ClientIp { get; init; }

    [JsonPropertyName("userId")]
    public int? UserId { get; init; }

    /// <summary>Specific VPOS code to route through, as configured in the Treps portal.</summary>
    [JsonPropertyName("vpos_code")]
    public string? VposCode { get; init; }

    /// <summary>Retry with an alternate POS on bank failure.</summary>
    [JsonPropertyName("retry_fail")]
    public bool? RetryFail { get; init; }

    [JsonPropertyName("customer_commission_plan_code")]
    public string? CustomerCommissionPlanCode { get; init; }

    /// <summary>Mail Order / Telephone Order transaction. Default: false.</summary>
    [JsonPropertyName("is_moto")]
    public bool? IsMoto { get; init; }

    [JsonPropertyName("card")]
    public required Card Card { get; init; }

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

/// <summary>Shared response fields for <c>Sale</c>, <c>PreAuth</c>, and 3D Secure <c>Complete</c>.</summary>
public record PaymentResponseData
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }

    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("secure_flag")]
    public required int SecureFlag { get; init; }

    [JsonPropertyName("secure_type")]
    public string? SecureType { get; init; }

    [JsonPropertyName("bank_vpos_code")]
    public required string BankVposCode { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("card_amount")]
    public required decimal CardAmount { get; init; }

    [JsonPropertyName("point_amount")]
    public required decimal PointAmount { get; init; }

    [JsonPropertyName("interest_amount")]
    public required decimal InterestAmount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("installment")]
    public required int Installment { get; init; }

    [JsonPropertyName("cost_rate")]
    public required decimal CostRate { get; init; }

    [JsonPropertyName("sale_rate")]
    public required decimal SaleRate { get; init; }

    [JsonPropertyName("return_url")]
    public required string ReturnUrl { get; init; }

    [JsonPropertyName("client_ip")]
    public required string ClientIp { get; init; }

    [JsonPropertyName("payment_status")]
    public required int PaymentStatus { get; init; }

    [JsonPropertyName("payment_status_code")]
    public required string PaymentStatusCode { get; init; }

    [JsonPropertyName("payment_status_message")]
    public required string PaymentStatusMessage { get; init; }

    [JsonPropertyName("bank_reference_id")]
    public string? BankReferenceId { get; init; }

    [JsonPropertyName("bank_auth_code")]
    public string? BankAuthCode { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public string? BankBatchNumber { get; init; }

    [JsonPropertyName("process_completed_date")]
    public required string ProcessCompletedDate { get; init; }

    [JsonPropertyName("have_sub_merchant_transaction_flag")]
    public required int HaveSubMerchantTransactionFlag { get; init; }

    [JsonPropertyName("created_user_id")]
    public required int CreatedUserId { get; init; }

    [JsonPropertyName("stored_card")]
    public object? StoredCard { get; init; }

    [JsonPropertyName("insert_date")]
    public required string InsertDate { get; init; }
}
