using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>POST /api/references/bin</summary>
public record BinQueryRequest
{
    /// <summary>The card's BIN, first 6-8 digits.</summary>
    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }
}

public record BinQueryResponseData
{
    [JsonPropertyName("bank_bic")]
    public required string BankBic { get; init; }

    [JsonPropertyName("card_brand")]
    public required string CardBrand { get; init; }

    [JsonPropertyName("card_network")]
    public required string CardNetwork { get; init; }

    [JsonPropertyName("card_country")]
    public required string CardCountry { get; init; }

    [JsonPropertyName("card_type")]
    public required string CardType { get; init; }

    [JsonPropertyName("business_card")]
    public required bool BusinessCard { get; init; }

    [JsonPropertyName("virtual_card")]
    public required bool VirtualCard { get; init; }

    [JsonPropertyName("is_installment_supported")]
    public required bool IsInstallmentSupported { get; init; }
}

/// <summary>POST /api/report/transaction-detail</summary>
public record TransactionDetailQueryRequest
{
    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }
}

public record TransactionDetailQueryResponseData
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

    [JsonPropertyName("secure_flag")]
    public required int SecureFlag { get; init; }

    [JsonPropertyName("hosted_card_token")]
    public required string HostedCardToken { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("transaction_amount")]
    public required decimal TransactionAmount { get; init; }

    [JsonPropertyName("transaction_status")]
    public required int TransactionStatus { get; init; }

    [JsonPropertyName("transaction_status_code")]
    public string? TransactionStatusCode { get; init; }

    [JsonPropertyName("bank_reference_id")]
    public string? BankReferenceId { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public string? BankBatchNumber { get; init; }

    [JsonPropertyName("result_code")]
    public string? ResultCode { get; init; }

    [JsonPropertyName("result_message")]
    public string? ResultMessage { get; init; }

    [JsonPropertyName("parent_transaction_sub_id")]
    public required int ParentTransactionSubId { get; init; }

    [JsonPropertyName("created_user_id")]
    public required int CreatedUserId { get; init; }

    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("status")]
    public required int Status { get; init; }

    [JsonPropertyName("insert_date")]
    public required string InsertDate { get; init; }

    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    [JsonPropertyName("card_bank_owner_name")]
    public required string CardBankOwnerName { get; init; }

    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public required string CardLastFour { get; init; }

    [JsonPropertyName("card_number")]
    public string? CardNumber { get; init; }

    [JsonPropertyName("card_country_code")]
    public required string CardCountryCode { get; init; }

    [JsonPropertyName("card_bank_code")]
    public required string CardBankCode { get; init; }

    [JsonPropertyName("card_type")]
    public required string CardType { get; init; }

    [JsonPropertyName("card_network")]
    public required string CardNetwork { get; init; }

    [JsonPropertyName("bank_id")]
    public required int BankId { get; init; }

    [JsonPropertyName("bank_vpos_id")]
    public required int BankVposId { get; init; }

    [JsonPropertyName("bank_vpos_code")]
    public required string BankVposCode { get; init; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }
}

/// <summary>POST /api/report/transaction-report</summary>
public record TransactionReportQueryRequest
{
    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }

    /// <summary>e.g. 1 = Sale.</summary>
    [JsonPropertyName("transaction_type")]
    public int? TransactionType { get; init; }

    /// <summary>e.g. 10 = Successful.</summary>
    [JsonPropertyName("transaction_status")]
    public int? TransactionStatus { get; init; }

    [JsonPropertyName("card_bin")]
    public string? CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public string? CardLastFour { get; init; }

    /// <summary>1 = yes, 0 = no.</summary>
    [JsonPropertyName("is_3d")]
    public int? Is3D { get; init; }

    /// <summary>1 = yes, 0 = no. Keeps the documented inner-capital-C spelling.</summary>
    [JsonPropertyName("is_savedCard")]
    public int? IsSavedCard { get; init; }

    [JsonPropertyName("vpos_id")]
    public string? VposId { get; init; }
}

public record TransactionReportItem
{
    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("transaction_type")]
    public required string TransactionType { get; init; }

    [JsonPropertyName("vpos_id")]
    public required string VposId { get; init; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }

    [JsonPropertyName("vpos_code")]
    public required string VposCode { get; init; }

    [JsonPropertyName("external_oid")]
    public required string ExternalOid { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("bank_auth_code")]
    public required string BankAuthCode { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public required string BankBatchNumber { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("payment_status")]
    public required string PaymentStatus { get; init; }

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("transaction_date")]
    public required string TransactionDate { get; init; }

    [JsonPropertyName("order_date")]
    public required string OrderDate { get; init; }

    [JsonPropertyName("payment_date")]
    public required string PaymentDate { get; init; }

    [JsonPropertyName("transaction_amount")]
    public required decimal TransactionAmount { get; init; }

    [JsonPropertyName("total")]
    public required decimal Total { get; init; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    [JsonPropertyName("customer_email")]
    public string? CustomerEmail { get; init; }

    [JsonPropertyName("customer_name")]
    public string? CustomerName { get; init; }

    [JsonPropertyName("card_bank")]
    public required string CardBank { get; init; }

    [JsonPropertyName("card_type")]
    public required string CardType { get; init; }

    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public required string CardLastFour { get; init; }

    [JsonPropertyName("is_saved_card")]
    public required bool IsSavedCard { get; init; }

    [JsonPropertyName("is_secure")]
    public required int IsSecure { get; init; }

    [JsonPropertyName("secure_result")]
    public string? SecureResult { get; init; }

    [JsonPropertyName("result_code")]
    public required string ResultCode { get; init; }

    [JsonPropertyName("result_message")]
    public required string ResultMessage { get; init; }

    [JsonPropertyName("bank_reference_id")]
    public required string BankReferenceId { get; init; }

    [JsonPropertyName("bank_id")]
    public required int BankId { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }
}

/// <summary>POST /api/report/order-detail</summary>
public record OrderDetailQueryRequest
{
    /// <summary>Required if OrderId is omitted.</summary>
    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; init; }

    /// <summary>Required if ExternalOrderId is omitted.</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; init; }
}

public record OrderDetailAddress
{
    [JsonPropertyName("order_id")]
    public required int OrderId { get; init; }

    /// <summary>Address type, e.g. 1 = billing.</summary>
    [JsonPropertyName("type")]
    public required int Type { get; init; }

    [JsonPropertyName("city")]
    public required string City { get; init; }

    [JsonPropertyName("country")]
    public required string Country { get; init; }

    [JsonPropertyName("address")]
    public required string AddressLine { get; init; }
}

public record OrderDetailProduct
{
    [JsonPropertyName("order_id")]
    public required int OrderId { get; init; }

    [JsonPropertyName("product_id")]
    public required string ProductId { get; init; }

    [JsonPropertyName("category")]
    public required string Category { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("price")]
    public required decimal Price { get; init; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; init; }
}

public record OrderDetailPayment
{
    [JsonPropertyName("bank_reference_id")]
    public string? BankReferenceId { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public string? BankBatchNumber { get; init; }

    [JsonPropertyName("payment_request_type")]
    public required int PaymentRequestType { get; init; }

    [JsonPropertyName("created_user_id")]
    public required int CreatedUserId { get; init; }

    [JsonPropertyName("order_db_id")]
    public required int OrderDbId { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("interest_amount")]
    public required decimal InterestAmount { get; init; }

    /// <summary>Documented field name as-is (looks like a typo-ish duplicate of ResultMessage).</summary>
    [JsonPropertyName("result_message2")]
    public string? ResultMessage2 { get; init; }

    [JsonPropertyName("card_network")]
    public required string CardNetwork { get; init; }

    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    [JsonPropertyName("secure_flag")]
    public required int SecureFlag { get; init; }

    [JsonPropertyName("bank_id")]
    public int? BankId { get; init; }

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("external_transaction_id")]
    public required string ExternalTransactionId { get; init; }

    [JsonPropertyName("card_last_four")]
    public required string CardLastFour { get; init; }

    [JsonPropertyName("payment_status_code")]
    public required string PaymentStatusCode { get; init; }

    [JsonPropertyName("payment_status_message")]
    public string? PaymentStatusMessage { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("payment_date")]
    public required string PaymentDate { get; init; }

    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }

    [JsonPropertyName("sale_rate")]
    public required decimal SaleRate { get; init; }

    [JsonPropertyName("result_message")]
    public required string ResultMessage { get; init; }

    [JsonPropertyName("bank_auth_code")]
    public required string BankAuthCode { get; init; }

    [JsonPropertyName("payment_status")]
    public required int PaymentStatus { get; init; }

    [JsonPropertyName("payment_type")]
    public required int PaymentType { get; init; }

    [JsonPropertyName("is_saved_card")]
    public required bool IsSavedCard { get; init; }
}

public record OrderDetailBuyer
{
    [JsonPropertyName("order_id")]
    public required int OrderId { get; init; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }
}

public record OrderDetailOrder
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("buyer")]
    public required OrderDetailBuyer Buyer { get; init; }

    [JsonPropertyName("addresses")]
    public required List<OrderDetailAddress> Addresses { get; init; }

    [JsonPropertyName("products")]
    public required List<OrderDetailProduct> Products { get; init; }

    [JsonPropertyName("sub_merchants")]
    public required List<object> SubMerchants { get; init; }

    /// <summary>Always empty in practice — the populated list is the sibling top-level Payments on <see cref="OrderDetailQueryResponseData"/>.</summary>
    [JsonPropertyName("payments")]
    public required List<OrderDetailPayment> Payments { get; init; }

    [JsonPropertyName("order_db_id")]
    public required int OrderDbId { get; init; }

    [JsonPropertyName("order_date")]
    public required string OrderDate { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }
}

public record OrderDetailQueryResponseData
{
    [JsonPropertyName("order")]
    public required OrderDetailOrder Order { get; init; }

    [JsonPropertyName("payments")]
    public required List<OrderDetailPayment> Payments { get; init; }
}

/// <summary>POST /api/report/order-report</summary>
public record OrderReportQueryRequest
{
    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }

    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("start_date")]
    public string? StartDate { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("end_date")]
    public string? EndDate { get; init; }
}

public record OrderReportItem
{
    [JsonPropertyName("merchant_id")]
    public int? MerchantId { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("bank_batch_number")]
    public string? BankBatchNumber { get; init; }

    [JsonPropertyName("payment_status")]
    public required int PaymentStatus { get; init; }

    [JsonPropertyName("secure_flag")]
    public required int SecureFlag { get; init; }

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("payment_type")]
    public required int PaymentType { get; init; }

    [JsonPropertyName("order_date")]
    public required string OrderDate { get; init; }

    [JsonPropertyName("payment_date")]
    public required string PaymentDate { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; init; }

    [JsonPropertyName("customer_email")]
    public string? CustomerEmail { get; init; }

    [JsonPropertyName("card_holder_name")]
    public required string CardHolderName { get; init; }

    [JsonPropertyName("bank_reference_id")]
    public string? BankReferenceId { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("bank_vpos_code")]
    public required string BankVposCode { get; init; }

    [JsonPropertyName("bank_vpos_id")]
    public required int BankVposId { get; init; }
}

public record OrderReportQueryResponseData
{
    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("data")]
    public required List<OrderReportItem> Data { get; init; }
}

public record CustomerCommissionScheme
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("entity_id")]
    public required int EntityId { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("code")]
    public required string Code { get; init; }

    [JsonPropertyName("separate_card_brands_flag")]
    public required int SeparateCardBrandsFlag { get; init; }

    [JsonPropertyName("eff_start_date")]
    public required string EffStartDate { get; init; }

    [JsonPropertyName("eff_end_date")]
    public required string EffEndDate { get; init; }

    [JsonPropertyName("items")]
    public object? Items { get; init; }
}

public record CustomerCommissionItem
{
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("installment_cnt")]
    public required int InstallmentCnt { get; init; }

    [JsonPropertyName("card_brand_code")]
    public required string CardBrandCode { get; init; }

    [JsonPropertyName("card_network")]
    public required string CardNetwork { get; init; }

    [JsonPropertyName("card_country_code")]
    public required string CardCountryCode { get; init; }

    [JsonPropertyName("commission_rate")]
    public required decimal CommissionRate { get; init; }
}

/// <summary>GET /api/commission/customercommissionitemcode/{code}</summary>
public record CustomerCommissionItemQueryResponseData
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("entity_id")]
    public required int EntityId { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("code")]
    public required string Code { get; init; }

    [JsonPropertyName("separate_card_brands_flag")]
    public required int SeparateCardBrandsFlag { get; init; }

    [JsonPropertyName("eff_start_date")]
    public required string EffStartDate { get; init; }

    [JsonPropertyName("eff_end_date")]
    public required string EffEndDate { get; init; }

    [JsonPropertyName("items")]
    public required List<CustomerCommissionItem> Items { get; init; }
}

/// <summary>POST /api/commission/getinstallmentinfo</summary>
public record InstallmentQueryRequest
{
    /// <summary>Card BIN, first 6-8 digits. Documented as a number elsewhere but examples send it as a string — use a string.</summary>
    [JsonPropertyName("bin")]
    public required string Bin { get; init; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; init; }

    /// <summary>e.g. TRY, USD, EUR, GBP.</summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    /// <summary>Customer's installment plan code. Pass an empty string to only get the single-payment (peşin) option.</summary>
    [JsonPropertyName("planCode")]
    public required string PlanCode { get; init; }

    /// <summary>Restrict to a specific VPOS's installment options.</summary>
    [JsonPropertyName("vposCode")]
    public string? VposCode { get; init; }
}

public record InstallmentOption
{
    [JsonPropertyName("installment_price")]
    public required decimal InstallmentPrice { get; init; }

    [JsonPropertyName("total_price")]
    public required decimal TotalPrice { get; init; }

    [JsonPropertyName("installment_cnt")]
    public required int InstallmentCnt { get; init; }
}

public record InstallmentQueryResponseData
{
    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("brand")]
    public required string Brand { get; init; }

    [JsonPropertyName("network")]
    public required string Network { get; init; }

    [JsonPropertyName("country")]
    public required string Country { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("force3ds")]
    public required bool Force3Ds { get; init; }

    [JsonPropertyName("installments")]
    public required List<InstallmentOption> Installments { get; init; }
}

/// <summary>POST /api/card/search — searches saved/tokenized cards.</summary>
public record CardQueryRequest
{
    [JsonPropertyName("customer_code")]
    public required string CustomerCode { get; init; }

    [JsonPropertyName("card_token")]
    public string? CardToken { get; init; }

    /// <summary>Not in the documented parameter table, but accepted (mirrors the response's CardOwnerName).</summary>
    [JsonPropertyName("card_owner_name")]
    public string? CardOwnerName { get; init; }

    [JsonPropertyName("card_alias")]
    public string? CardAlias { get; init; }

    [JsonPropertyName("card_bin")]
    public string? CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public string? CardLastFour { get; init; }

    /// <summary>MM/YYYY</summary>
    [JsonPropertyName("card_expire_begin")]
    public string? CardExpireBegin { get; init; }

    /// <summary>MM/YYYY</summary>
    [JsonPropertyName("card_expire_end")]
    public string? CardExpireEnd { get; init; }

    [JsonPropertyName("card_network")]
    public string? CardNetwork { get; init; }

    [JsonPropertyName("card_type")]
    public string? CardType { get; init; }

    [JsonPropertyName("card_brand")]
    public string? CardBrand { get; init; }

    [JsonPropertyName("card_bank_bic")]
    public string? CardBankBic { get; init; }

    /// <summary>Not in the documented parameter table, but accepted (mirrors the response's CardReferenceCode).</summary>
    [JsonPropertyName("card_reference_code")]
    public string? CardReferenceCode { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("create_date_begin")]
    public string? CreateDateBegin { get; init; }

    /// <summary>ISO 8601.</summary>
    [JsonPropertyName("create_date_end")]
    public string? CreateDateEnd { get; init; }

    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }
}

public record SavedCard
{
    [JsonPropertyName("customer_code")]
    public required string CustomerCode { get; init; }

    [JsonPropertyName("merchant_id")]
    public required int MerchantId { get; init; }

    [JsonPropertyName("card_token")]
    public required string CardToken { get; init; }

    [JsonPropertyName("card_network")]
    public required string CardNetwork { get; init; }

    [JsonPropertyName("card_type")]
    public required string CardType { get; init; }

    [JsonPropertyName("card_country")]
    public required string CardCountry { get; init; }

    [JsonPropertyName("create_date")]
    public required string CreateDate { get; init; }

    [JsonPropertyName("bank_bic")]
    public required string BankBic { get; init; }

    [JsonPropertyName("card_brand")]
    public required string CardBrand { get; init; }

    [JsonPropertyName("business_card")]
    public required bool BusinessCard { get; init; }

    [JsonPropertyName("virtual_card")]
    public required bool VirtualCard { get; init; }

    [JsonPropertyName("card_owner_name")]
    public required string CardOwnerName { get; init; }

    [JsonPropertyName("card_reference_code")]
    public required string CardReferenceCode { get; init; }

    [JsonPropertyName("card_alias")]
    public required string CardAlias { get; init; }

    [JsonPropertyName("card_bin")]
    public required string CardBin { get; init; }

    [JsonPropertyName("card_last_four")]
    public required string CardLastFour { get; init; }

    /// <summary>MM/YYYY</summary>
    [JsonPropertyName("card_expire_date")]
    public required string CardExpireDate { get; init; }

    [JsonPropertyName("is_installment_supported")]
    public required bool IsInstallmentSupported { get; init; }
}

public record CardQueryResponseData
{
    [JsonPropertyName("page")]
    public required int Page { get; init; }

    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }

    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("cards")]
    public required List<SavedCard> Cards { get; init; }
}
