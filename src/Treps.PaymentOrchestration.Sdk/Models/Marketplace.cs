using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

// ---------------------------------------------------------------------
// Sub-merchants — /api/marketplace/submerchant/*
// ---------------------------------------------------------------------

/// <summary>POST /api/marketplace/submerchant/add</summary>
public record SubMerchantAddRequest
{
    /// <summary>Unique identifier you choose for this sub-merchant.</summary>
    [JsonPropertyName("reference_id")]
    public required string ReferenceId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("commercial_name")]
    public string? CommercialName { get; init; }

    /// <summary>Company type enum (see the docs site for the full table).</summary>
    [JsonPropertyName("sole_prop_flag")]
    public required int SolePropFlag { get; init; }

    [JsonPropertyName("tax_office")]
    public required string TaxOffice { get; init; }

    /// <summary>VKN (tax id, corporations) or TCKN (citizenship number, sole proprietors).</summary>
    [JsonPropertyName("vkn_tckn")]
    public required string VknTckn { get; init; }

    [JsonPropertyName("address")]
    public required string Address { get; init; }

    [JsonPropertyName("district")]
    public required string District { get; init; }

    [JsonPropertyName("province_code")]
    public required string ProvinceCode { get; init; }

    /// <summary>ISO 3166-1 alpha-3, e.g. "TUR".</summary>
    [JsonPropertyName("country_alpha3")]
    public required string CountryAlpha3 { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("phone")]
    public required string Phone { get; init; }

    /// <summary>How settlement funds are transferred to the sub-merchant, e.g. 1 = IBAN.</summary>
    [JsonPropertyName("accounting_transfer_method")]
    public required int AccountingTransferMethod { get; init; }

    [JsonPropertyName("iban_owner_name")]
    public required string IbanOwnerName { get; init; }

    [JsonPropertyName("iban")]
    public required string Iban { get; init; }

    [JsonPropertyName("wallet_account_code")]
    public string? WalletAccountCode { get; init; }

    [JsonPropertyName("contact_name")]
    public required string ContactName { get; init; }

    [JsonPropertyName("contact_surname")]
    public required string ContactSurname { get; init; }

    /// <summary>Valör (settlement hold) day count.</summary>
    [JsonPropertyName("blocked_day_count")]
    public required int BlockedDayCount { get; init; }

    /// <summary>1 = active, 0 = passive.</summary>
    [JsonPropertyName("status")]
    public required int Status { get; init; }
}

/// <summary>
/// POST /api/marketplace/submerchant/update — same fields as <see cref="SubMerchantAddRequest"/>
/// except only <see cref="ReferenceId"/> is required; every other field is optional and only
/// updates the sub-merchant when provided.
/// </summary>
public record SubMerchantUpdateRequest
{
    [JsonPropertyName("reference_id")]
    public required string ReferenceId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("commercial_name")]
    public string? CommercialName { get; init; }

    [JsonPropertyName("sole_prop_flag")]
    public int? SolePropFlag { get; init; }

    [JsonPropertyName("tax_office")]
    public string? TaxOffice { get; init; }

    [JsonPropertyName("vkn_tckn")]
    public string? VknTckn { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("district")]
    public string? District { get; init; }

    [JsonPropertyName("province_code")]
    public string? ProvinceCode { get; init; }

    [JsonPropertyName("country_alpha3")]
    public string? CountryAlpha3 { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("phone")]
    public string? Phone { get; init; }

    [JsonPropertyName("accounting_transfer_method")]
    public int? AccountingTransferMethod { get; init; }

    [JsonPropertyName("iban_owner_name")]
    public string? IbanOwnerName { get; init; }

    [JsonPropertyName("iban")]
    public string? Iban { get; init; }

    [JsonPropertyName("wallet_account_code")]
    public string? WalletAccountCode { get; init; }

    [JsonPropertyName("contact_name")]
    public string? ContactName { get; init; }

    [JsonPropertyName("contact_surname")]
    public string? ContactSurname { get; init; }

    [JsonPropertyName("blocked_day_count")]
    public int? BlockedDayCount { get; init; }

    [JsonPropertyName("status")]
    public int? Status { get; init; }
}

/// <summary>POST /api/marketplace/submerchant/get</summary>
public record SubMerchantGetRequest
{
    [JsonPropertyName("reference_id")]
    public required string ReferenceId { get; init; }
}

/// <summary>
/// Full sub-merchant record as returned by add/update/get/find — the request fields plus a few
/// server-computed descriptions and the creation timestamp.
/// </summary>
public record SubMerchantResponseData
{
    [JsonPropertyName("reference_id")]
    public required string ReferenceId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("commercial_name")]
    public string? CommercialName { get; init; }

    [JsonPropertyName("sole_prop_flag")]
    public required int SolePropFlag { get; init; }

    [JsonPropertyName("sole_prop_flag_desc")]
    public required string SolePropFlagDesc { get; init; }

    [JsonPropertyName("tax_office")]
    public required string TaxOffice { get; init; }

    [JsonPropertyName("vkn_tckn")]
    public required string VknTckn { get; init; }

    [JsonPropertyName("address")]
    public required string Address { get; init; }

    [JsonPropertyName("district")]
    public required string District { get; init; }

    [JsonPropertyName("province_code")]
    public required string ProvinceCode { get; init; }

    [JsonPropertyName("country_alpha3")]
    public required string CountryAlpha3 { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("phone")]
    public required string Phone { get; init; }

    [JsonPropertyName("accounting_transfer_method")]
    public required int AccountingTransferMethod { get; init; }

    [JsonPropertyName("accounting_transfer_method_desc")]
    public required string AccountingTransferMethodDesc { get; init; }

    [JsonPropertyName("iban_owner_name")]
    public required string IbanOwnerName { get; init; }

    [JsonPropertyName("iban")]
    public required string Iban { get; init; }

    [JsonPropertyName("wallet_account_code")]
    public string? WalletAccountCode { get; init; }

    [JsonPropertyName("contact_name")]
    public required string ContactName { get; init; }

    [JsonPropertyName("contact_surname")]
    public required string ContactSurname { get; init; }

    [JsonPropertyName("blocked_day_count")]
    public required int BlockedDayCount { get; init; }

    [JsonPropertyName("status")]
    public required int Status { get; init; }

    /// <summary>ISO datetime string.</summary>
    [JsonPropertyName("insert_date")]
    public required string InsertDate { get; init; }
}

/// <summary>POST /api/marketplace/submerchant/find — paginated, filterable sub-merchant search.</summary>
public record SubMerchantFindRequest
{
    [JsonPropertyName("reference_id")]
    public string? ReferenceId { get; init; }

    [JsonPropertyName("reference_ids")]
    public List<string>? ReferenceIds { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("vkn_tckn")]
    public string? VknTckn { get; init; }

    [JsonPropertyName("status")]
    public int? Status { get; init; }

    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }
}

public record SubMerchantFindResponseData
{
    [JsonPropertyName("items")]
    public required List<SubMerchantResponseData> Items { get; init; }

    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("page")]
    public required int Page { get; init; }

    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }
}

// ---------------------------------------------------------------------
// Marketplace settings — /api/marketplace/config
// ---------------------------------------------------------------------

/// <summary>
/// POST /api/marketplace/config — write-only: there is no corresponding GET/read endpoint for
/// this setting on the backend as of this writing, so there's nothing to expose beyond
/// <see cref="Resources.MarketplaceResource.ConfigAsync"/> itself.
/// </summary>
public record MarketplaceConfigRequest
{
    /// <summary>1 = require manual approval before a marketplace order's payment can be transferred, 0 = don't.</summary>
    [JsonPropertyName("payment_transfer_approve_required")]
    public required int PaymentTransferApproveRequired { get; init; }
}

// ---------------------------------------------------------------------
// Orders — /api/marketplace/order/*
// ---------------------------------------------------------------------

/// <summary>One entry of the batch body posted to POST /api/marketplace/order/approve.</summary>
public record OrderApproveItem
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("partial_approve")]
    public required bool PartialApprove { get; init; }

    [JsonPropertyName("approve_amount")]
    public required decimal ApproveAmount { get; init; }
}

/// <summary>One entry of the batch body posted to POST /api/marketplace/order/pay.</summary>
public record OrderPayItem
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("partial_pay")]
    public required bool PartialPay { get; init; }

    [JsonPropertyName("pay_amount")]
    public required decimal PayAmount { get; init; }

    [JsonPropertyName("payment_reference_codes")]
    public required List<string> PaymentReferenceCodes { get; init; }
}

/// <summary>One entry of the batch body posted to POST /api/marketplace/order/pay/allocate.</summary>
public record OrderPayAllocateItem
{
    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("payment_reference_codes")]
    public required List<string> PaymentReferenceCodes { get; init; }
}

/// <summary>A single sub-merchant's applied allocations within an <see cref="OrderPayAllocateResponseData"/>.</summary>
public record OrderPayAllocateAllocation
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("applied_amount")]
    public required decimal AppliedAmount { get; init; }
}

/// <summary>Per-sub-merchant result within an <see cref="OrderPayAllocateResponseData"/>.</summary>
public record OrderPayAllocateResultItem
{
    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("success")]
    public required bool Success { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("allocations")]
    public required List<OrderPayAllocateAllocation> Allocations { get; init; }
}

/// <summary>
/// POST /api/marketplace/order/pay/allocate response.
///
/// <para><b>This operation is atomic.</b> If <see cref="Success"/> is <c>false</c>, NONE of the
/// entries in <see cref="Items"/> were actually applied — the whole batch is rolled back — even
/// if some individual <see cref="OrderPayAllocateResultItem.Success"/> values are <c>true</c>.
/// A row only really succeeded when both its own <c>Items[i].Success == true</c> AND the
/// top-level <see cref="Success"/> is <c>true</c>. Never treat a per-item success flag as
/// meaningful on its own.</para>
/// </summary>
public record OrderPayAllocateResponseData
{
    [JsonPropertyName("success")]
    public required bool Success { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("items")]
    public required List<OrderPayAllocateResultItem> Items { get; init; }
}

/// <summary>POST /api/marketplace/order/seized</summary>
public record OrderSeizedRequest
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("seized_amount")]
    public required decimal SeizedAmount { get; init; }

    [JsonPropertyName("seized_reason")]
    public required string SeizedReason { get; init; }
}

/// <summary>
/// POST /api/marketplace/order/refund — refunds a sub-merchant's allocation of a marketplace
/// order. Distinct from <see cref="RefundRequest"/> (POST /api/payment/refund), which refunds
/// the top-level payment and can itself carry a per-sub-merchant allocation list via
/// <see cref="RefundRequest.SubMerchants"/>.
/// </summary>
public record OrderRefundRequest
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("refund_amount")]
    public required decimal RefundAmount { get; init; }

    [JsonPropertyName("reason")]
    public required string Reason { get; init; }
}

/// <summary>POST /api/marketplace/order/collectdept</summary>
public record OrderCollectDebtRequest
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("collected_amount")]
    public required decimal CollectedAmount { get; init; }
}

/// <summary>POST /api/marketplace/order/cancel</summary>
public record OrderCancelRequest
{
    [JsonPropertyName("oid")]
    public required string Oid { get; init; }
}

// ---------------------------------------------------------------------
// Settlement reporting — /api/marketplace/settlement/*
// ---------------------------------------------------------------------

/// <summary>POST /api/marketplace/settlement/summary</summary>
public record SettlementSummaryRequest
{
    [JsonPropertyName("sub_merchant_reference_ids")]
    public List<string>? SubMerchantReferenceIds { get; init; }

    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }
}

public record SettlementSummaryItem
{
    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("total_amount")]
    public required decimal TotalAmount { get; init; }

    [JsonPropertyName("total_approved")]
    public required decimal TotalApproved { get; init; }

    [JsonPropertyName("total_seized")]
    public required decimal TotalSeized { get; init; }

    [JsonPropertyName("total_refunded")]
    public required decimal TotalRefunded { get; init; }

    [JsonPropertyName("total_paid")]
    public required decimal TotalPaid { get; init; }

    [JsonPropertyName("net_balance")]
    public required decimal NetBalance { get; init; }

    [JsonPropertyName("earned_balance")]
    public required decimal EarnedBalance { get; init; }

    [JsonPropertyName("not_earned_balance")]
    public required decimal NotEarnedBalance { get; init; }
}

public record SettlementSummaryResponseData
{
    [JsonPropertyName("items")]
    public required List<SettlementSummaryItem> Items { get; init; }

    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("page")]
    public required int Page { get; init; }

    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }
}

/// <summary>Filter payload nested inside <see cref="SettlementSummaryExportRequest"/>.</summary>
public record SettlementSummaryExportFilter
{
    [JsonPropertyName("sub_merchant_reference_ids")]
    public List<string>? SubMerchantReferenceIds { get; init; }
}

/// <summary>
/// POST /api/marketplace/settlement/summary/export — kicks off an asynchronous export job of
/// <see cref="Resources.SettlementResource.SummaryAsync"/>'s data. Track and fetch it via
/// <c>client.DownloadJobs</c> (filter by <c>report_type</c> = SubMerchantSettlement) once its
/// job_status is Completed.
/// </summary>
public record SettlementSummaryExportRequest
{
    [JsonPropertyName("report_name")]
    public required string ReportName { get; init; }

    [JsonPropertyName("filter")]
    public SettlementSummaryExportFilter? Filter { get; init; }
}

/// <summary>POST /api/marketplace/settlement/detail — paginated, filterable order-level settlement line items.</summary>
public record SettlementDetailRequest
{
    [JsonPropertyName("sub_merchant_reference_ids")]
    public List<string>? SubMerchantReferenceIds { get; init; }

    [JsonPropertyName("oid")]
    public string? Oid { get; init; }

    /// <summary>0 = All, 1 = Paid, 2 = Unpaid, 3 = AwaitingValor.</summary>
    [JsonPropertyName("status_filter")]
    public int? StatusFilter { get; init; }

    [JsonPropertyName("order_date_from")]
    public DateTime? OrderDateFrom { get; init; }

    [JsonPropertyName("order_date_to")]
    public DateTime? OrderDateTo { get; init; }

    /// <summary>0 = None, 1 = Full, 2 = Partial.</summary>
    [JsonPropertyName("approval_status_filter")]
    public int? ApprovalStatusFilter { get; init; }

    [JsonPropertyName("min_unpaid_amount")]
    public decimal? MinUnpaidAmount { get; init; }

    [JsonPropertyName("max_unpaid_amount")]
    public decimal? MaxUnpaidAmount { get; init; }

    [JsonPropertyName("has_seized")]
    public bool? HasSeized { get; init; }

    [JsonPropertyName("has_refunded")]
    public bool? HasRefunded { get; init; }

    [JsonPropertyName("payment_reference_code")]
    public string? PaymentReferenceCode { get; init; }

    [JsonPropertyName("page")]
    public int? Page { get; init; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; init; }
}

public record SettlementDetailItem
{
    [JsonPropertyName("sub_merchant_reference_id")]
    public required string SubMerchantReferenceId { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("approval_amount")]
    public required decimal ApprovalAmount { get; init; }

    /// <summary>0 = None, 1 = Full, 2 = Partial.</summary>
    [JsonPropertyName("approval_status")]
    public required int ApprovalStatus { get; init; }

    [JsonPropertyName("seized_amount")]
    public required decimal SeizedAmount { get; init; }

    [JsonPropertyName("refunded_amount")]
    public required decimal RefundedAmount { get; init; }

    [JsonPropertyName("paid_amount")]
    public required decimal PaidAmount { get; init; }

    /// <summary>0 = None, 1 = Full, 2 = Partial.</summary>
    [JsonPropertyName("paid_status")]
    public required int PaidStatus { get; init; }

    [JsonPropertyName("unpaid_amount")]
    public required decimal UnpaidAmount { get; init; }

    [JsonPropertyName("insert_date")]
    public required DateTime InsertDate { get; init; }

    [JsonPropertyName("earned_date")]
    public required DateTime EarnedDate { get; init; }

    [JsonPropertyName("is_earned")]
    public required bool IsEarned { get; init; }

    [JsonPropertyName("payment_reference_numbers")]
    public required List<string> PaymentReferenceNumbers { get; init; }
}

public record SettlementDetailResponseData
{
    [JsonPropertyName("items")]
    public required List<SettlementDetailItem> Items { get; init; }

    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }

    [JsonPropertyName("page")]
    public required int Page { get; init; }

    [JsonPropertyName("page_size")]
    public required int PageSize { get; init; }
}
