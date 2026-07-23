using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

public record PaymentLinkBuyer
{
    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("surname")]
    public string? Surname { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; init; }
}

public record PaymentLinkProduct
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

/// <summary>POST /api/payment/createpaylink</summary>
public record PaymentLinkCreateRequest
{
    /// <summary>Label/name for the link.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>Unique reference for the order, up to 72 chars.</summary>
    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("customer_commission_plan_code")]
    public required string CustomerCommissionPlanCode { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    /// <summary>ISO currency code, e.g. TRY, USD, EUR, GBP.</summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    /// <summary>Auth=1, PreAuth=2.</summary>
    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("min_installment")]
    public required int MinInstallment { get; init; }

    /// <summary>ISO 8601 expiry for the link.</summary>
    [JsonPropertyName("expire_date")]
    public required string ExpireDate { get; init; }

    /// <summary>Forces the 3D Secure flow.</summary>
    [JsonPropertyName("secure_flag")]
    public int? SecureFlag { get; init; }

    [JsonPropertyName("max_installment")]
    public int? MaxInstallment { get; init; }

    /// <summary>1 = single-use (cannot be reused after a successful payment), 0 = multi-use.</summary>
    [JsonPropertyName("onetime_flag")]
    public int? OnetimeFlag { get; init; }

    [JsonPropertyName("vpos_code")]
    public string? VposCode { get; init; }

    [JsonPropertyName("buyer")]
    public PaymentLinkBuyer? Buyer { get; init; }

    /// <summary>Singular, unlike the Products[] array used by sale/hostedpage requests.</summary>
    [JsonPropertyName("product")]
    public PaymentLinkProduct? Product { get; init; }
}

public record PaymentLinkCreateResponseData
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }
}

/// <summary>POST /api/payment/getpaylink</summary>
public record PaymentLinkQueryRequest
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }
}

public record PaymentLinkQueryProduct
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

    [JsonPropertyName("link_payment_id")]
    public required int LinkPaymentId { get; init; }
}

public record PaymentLinkQueryBuyer
{
    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("surname")]
    public string? Surname { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; init; }

    [JsonPropertyName("link_payment_id")]
    public required int LinkPaymentId { get; init; }

    [JsonPropertyName("citizenship_number")]
    public string? CitizenshipNumber { get; init; }
}

public record PaymentLinkQueryResponseData
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("secure_flag")]
    public required int SecureFlag { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("customer_commission_plan_code")]
    public required string CustomerCommissionPlanCode { get; init; }

    [JsonPropertyName("expire_date")]
    public required string ExpireDate { get; init; }

    [JsonPropertyName("onetime_flag")]
    public required int OnetimeFlag { get; init; }

    [JsonPropertyName("created_user_id")]
    public required int CreatedUserId { get; init; }

    [JsonPropertyName("installment_options")]
    public object? InstallmentOptions { get; init; }

    [JsonPropertyName("min_installment")]
    public required int MinInstallment { get; init; }

    [JsonPropertyName("max_installment")]
    public required int MaxInstallment { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("reseller_id")]
    public int? ResellerId { get; init; }

    [JsonPropertyName("sub_reseller_id")]
    public int? SubResellerId { get; init; }

    [JsonPropertyName("order_ids")]
    public required List<string> OrderIds { get; init; }

    /// <summary>true once at least one successful payment has been made on this link.</summary>
    [JsonPropertyName("have_completed_order")]
    public required bool HaveCompletedOrder { get; init; }

    [JsonPropertyName("products")]
    public required List<PaymentLinkQueryProduct> Products { get; init; }

    [JsonPropertyName("buyer")]
    public required PaymentLinkQueryBuyer Buyer { get; init; }

    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// The docs are inconsistent about this enum: a reference panel describes 1=active/2=expired,
    /// but request examples and ListAsync()'s filter only ever show 0/1 in practice. Prefer
    /// HaveCompletedOrder and ExpireDate for real status checks.
    /// </summary>
    [JsonPropertyName("status")]
    public required int Status { get; init; }

    [JsonPropertyName("insert_date")]
    public required string InsertDate { get; init; }
}

/// <summary>POST /api/payment/listpaylinks — all filters are optional.</summary>
public record PaymentLinkListRequest
{
    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    /// <summary>See the Status caveat on <see cref="PaymentLinkQueryResponseData"/>.</summary>
    [JsonPropertyName("status")]
    public int? Status { get; init; }

    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; init; }

    [JsonPropertyName("link_payment_id")]
    public int? LinkPaymentId { get; init; }

    [JsonPropertyName("token")]
    public string? Token { get; init; }
}

public record PaymentLinkListItem
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("buyer_name")]
    public required string BuyerName { get; init; }

    [JsonPropertyName("buyer_surname")]
    public required string BuyerSurname { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("reseller_id")]
    public int? ResellerId { get; init; }

    [JsonPropertyName("sub_reseller_id")]
    public int? SubResellerId { get; init; }

    [JsonPropertyName("have_completed_order")]
    public required bool HaveCompletedOrder { get; init; }

    [JsonPropertyName("expire_date")]
    public required string ExpireDate { get; init; }

    [JsonPropertyName("status")]
    public required int Status { get; init; }

    [JsonPropertyName("onetime_flag")]
    public required int OnetimeFlag { get; init; }
}

public record PaymentLinkListResponseData
{
    [JsonPropertyName("page")]
    public required int Page { get; init; }

    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }

    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("data")]
    public required List<PaymentLinkListItem> Data { get; init; }
}
