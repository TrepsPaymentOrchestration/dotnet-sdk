using System.Text.Json.Serialization;

namespace Treps.PaymentOrchestration.Sdk.Models;

/// <summary>POST /api/payment/hostedpage with iframe_flag: 0 (Secure Payment Page / Hosted Page).</summary>
public record HostedPageInitRequest
{
    /// <summary>Unique reference for the order, up to 72 chars. Critical for duplicate detection.</summary>
    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    /// <summary>Decimal amount.</summary>
    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    /// <summary>ISO currency code, e.g. TRY, USD, EUR, GBP.</summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    /// <summary>Auth=1, PreAuth=2.</summary>
    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    /// <summary>HTTPS callback URL the result is POSTed to. HTTP is not supported.</summary>
    [JsonPropertyName("return_url")]
    public required string ReturnUrl { get; init; }

    [JsonPropertyName("min_installment")]
    public required int MinInstallment { get; init; }

    [JsonPropertyName("max_installment")]
    public int? MaxInstallment { get; init; }

    /// <summary>ISO 8601 expiry for the payment link/token.</summary>
    [JsonPropertyName("expire_date")]
    public required string ExpireDate { get; init; }

    [JsonPropertyName("customer_commission_plan_code")]
    public required string CustomerCommissionPlanCode { get; init; }

    [JsonPropertyName("lang")]
    public required string Lang { get; init; }

    /// <summary>Label for the "back to store" button shown after payment completes.</summary>
    [JsonPropertyName("return_button_text")]
    public required string ReturnButtonText { get; init; }

    /// <summary>Required when ReturnButtonText is set.</summary>
    [JsonPropertyName("return_button_url")]
    public required string ReturnButtonUrl { get; init; }

    /// <summary>Seconds to wait before auto-redirecting after payment completes.</summary>
    [JsonPropertyName("redirect_timeout")]
    public required int RedirectTimeout { get; init; }

    /// <summary>Forces the 3D Secure flow. If the portal requires 3D, SecureFlag: 0 is rejected.</summary>
    [JsonPropertyName("secure_flag")]
    public int? SecureFlag { get; init; }

    /// <summary>Require SMS OTP when paying with a saved card (only meaningful with SaveCard: true).</summary>
    [JsonPropertyName("stored_card_sms_otp")]
    public bool? StoredCardSmsOtp { get; init; }

    /// <summary>Offer the customer the option to save their card. 1 requires CardOwnerCustomerId.</summary>
    [JsonPropertyName("save_card")]
    public int? SaveCard { get; init; }

    /// <summary>Retry with an alternate POS on bank failure/timeout. Recommended.</summary>
    [JsonPropertyName("retry_fail")]
    public bool? RetryFail { get; init; }

    [JsonPropertyName("vpos_code")]
    public string? VposCode { get; init; }

    /// <summary>Must be 0 for the Hosted Page flow. Set automatically by <c>HostedPageResource.CreateAsync()</c>.</summary>
    [JsonPropertyName("iframe_flag")]
    public int IframeFlag { get; init; }

    /// <summary>1 = always redirect to ReturnUrl; 0 = keep the customer on the payment page after a failure.</summary>
    [JsonPropertyName("redirect_after_fail_payment")]
    public int? RedirectAfterFailPayment { get; init; }

    [JsonPropertyName("buyer")]
    public Buyer? Buyer { get; init; }

    [JsonPropertyName("products")]
    public List<Product>? Products { get; init; }

    [JsonPropertyName("billing_address")]
    public Address? BillingAddress { get; init; }

    [JsonPropertyName("shipping_address")]
    public Address? ShippingAddress { get; init; }
}

/// <summary>
/// Inline CSS customization for the embedded IFrame payment form. All keys are hyphenated,
/// matching the actual wire format used by the API (the underscored variants that appear in
/// some prose documentation are a docs inconsistency, not what the API accepts).
/// </summary>
public record IFrameCssVariables
{
    [JsonPropertyName("text-color")]
    public string? TextColor { get; init; }

    [JsonPropertyName("text-font-weight")]
    public string? TextFontWeight { get; init; }

    [JsonPropertyName("font-family")]
    public string? FontFamily { get; init; }

    [JsonPropertyName("font-size")]
    public string? FontSize { get; init; }

    [JsonPropertyName("input-bg")]
    public string? InputBg { get; init; }

    [JsonPropertyName("input-border")]
    public string? InputBorder { get; init; }

    [JsonPropertyName("input-radius")]
    public string? InputRadius { get; init; }

    [JsonPropertyName("input-padding")]
    public string? InputPadding { get; init; }

    [JsonPropertyName("input-color")]
    public string? InputColor { get; init; }

    [JsonPropertyName("input-font-weight")]
    public string? InputFontWeight { get; init; }

    [JsonPropertyName("button-background-color")]
    public string? ButtonBackgroundColor { get; init; }

    [JsonPropertyName("button-background-color-hover")]
    public string? ButtonBackgroundColorHover { get; init; }

    [JsonPropertyName("button-color")]
    public string? ButtonColor { get; init; }

    [JsonPropertyName("button-color-hover")]
    public string? ButtonColorHover { get; init; }

    [JsonPropertyName("button-padding")]
    public string? ButtonPadding { get; init; }

    [JsonPropertyName("button-border")]
    public string? ButtonBorder { get; init; }

    [JsonPropertyName("button-border-hover")]
    public string? ButtonBorderHover { get; init; }

    [JsonPropertyName("button-width")]
    public string? ButtonWidth { get; init; }

    [JsonPropertyName("button-max-width")]
    public string? ButtonMaxWidth { get; init; }

    [JsonPropertyName("button-transition")]
    public string? ButtonTransition { get; init; }

    [JsonPropertyName("button-container-text-align")]
    public string? ButtonContainerTextAlign { get; init; }

    [JsonPropertyName("button-container-margin-top")]
    public string? ButtonContainerMarginTop { get; init; }

    [JsonPropertyName("label-margin")]
    public string? LabelMargin { get; init; }

    [JsonPropertyName("installment-border-color")]
    public string? InstallmentBorderColor { get; init; }

    [JsonPropertyName("installment-selected-border-color")]
    public string? InstallmentSelectedBorderColor { get; init; }

    [JsonPropertyName("installment-selected-background-color")]
    public string? InstallmentSelectedBackgroundColor { get; init; }

    /// <summary>'0' or '1' as a string, matching the documented examples.</summary>
    [JsonPropertyName("hide-installments")]
    public string? HideInstallments { get; init; }

    /// <summary>'0' or '1' as a string, matching the documented examples.</summary>
    [JsonPropertyName("hide-pay-button")]
    public string? HidePayButton { get; init; }

    /// <summary>'0' or '1' as a string, matching the documented examples.</summary>
    [JsonPropertyName("hide-labels")]
    public string? HideLabels { get; init; }
}

/// <summary>POST /api/payment/hostedpage with iframe_flag: 1 (embedded IFrame checkout).</summary>
public record IFrameInitRequest
{
    /// <summary>Unique reference for the order, up to 50 chars.</summary>
    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    /// <summary>ISO currency code, e.g. TRY, USD, EUR, GBP.</summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    /// <summary>Forces the 3D Secure flow.</summary>
    [JsonPropertyName("secure_flag")]
    public int? SecureFlag { get; init; }

    /// <summary>Auth=1, PreAuth=2.</summary>
    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("min_installment")]
    public int? MinInstallment { get; init; }

    [JsonPropertyName("max_installment")]
    public int? MaxInstallment { get; init; }

    /// <summary>ISO 8601 expiry for the payment link/token.</summary>
    [JsonPropertyName("expire_date")]
    public string? ExpireDate { get; init; }

    /// <summary>HTTPS callback URL the result is POSTed to.</summary>
    [JsonPropertyName("return_url")]
    public required string ReturnUrl { get; init; }

    /// <summary>Retry with an alternate POS on bank failure.</summary>
    [JsonPropertyName("retry_fail")]
    public bool? RetryFail { get; init; }

    [JsonPropertyName("customer_commission_plan_code")]
    public string? CustomerCommissionPlanCode { get; init; }

    [JsonPropertyName("vpos_code")]
    public string? VposCode { get; init; }

    /// <summary>The URL of the web page the iframe is embedded into.</summary>
    [JsonPropertyName("iframe_web_uri")]
    public required string IframeWebUri { get; init; }

    /// <summary>UI language, e.g. 'tr'.</summary>
    [JsonPropertyName("lang")]
    public string? Lang { get; init; }

    [JsonPropertyName("css-variables")]
    public IFrameCssVariables? CssVariables { get; init; }

    [JsonPropertyName("buyer")]
    public Buyer? Buyer { get; init; }

    [JsonPropertyName("products")]
    public List<Product>? Products { get; init; }

    [JsonPropertyName("billing_address")]
    public Address? BillingAddress { get; init; }

    [JsonPropertyName("shipping_address")]
    public Address? ShippingAddress { get; init; }

    /// <summary>Must be 1 for the IFrame flow. Set automatically by <c>HostedPageResource.CreateIframeAsync()</c>.</summary>
    [JsonPropertyName("iframe_flag")]
    public int IframeFlag { get; init; }
}

public record HostedPageInitResponseData
{
    /// <summary>URL to redirect the customer's browser to.</summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    /// <summary>Token identifying this Hosted Page session; also usable to query its status.</summary>
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("expire_date")]
    public required string ExpireDate { get; init; }
}

public record HostedPageTransaction
{
    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; init; }

    [JsonPropertyName("transaction_amount")]
    public required decimal TransactionAmount { get; init; }

    [JsonPropertyName("transaction_status")]
    public required int TransactionStatus { get; init; }

    [JsonPropertyName("transaction_status_code")]
    public required string TransactionStatusCode { get; init; }

    [JsonPropertyName("result_code")]
    public required string ResultCode { get; init; }

    [JsonPropertyName("result_message")]
    public required string ResultMessage { get; init; }
}

public record HostedPagePayment
{
    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    [JsonPropertyName("transaction_type")]
    public required int TransactionType { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("payment_status")]
    public required int PaymentStatus { get; init; }

    [JsonPropertyName("payment_status_code")]
    public required string PaymentStatusCode { get; init; }

    [JsonPropertyName("payment_status_message")]
    public required string PaymentStatusMessage { get; init; }

    [JsonPropertyName("bank_auth_code")]
    public required string BankAuthCode { get; init; }

    [JsonPropertyName("transactions")]
    public required List<HostedPageTransaction> Transactions { get; init; }
}

public record HostedPageOrderStatus
{
    [JsonPropertyName("payments")]
    public required List<HostedPagePayment> Payments { get; init; }

    [JsonPropertyName("order_completed")]
    public required bool OrderCompleted { get; init; }

    [JsonPropertyName("order_success_amount")]
    public required decimal OrderSuccessAmount { get; init; }
}

/// <summary>GET /api/payment/hostedpage/{token}</summary>
public record HostedPageQueryResponseData
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("external_order_id")]
    public required string ExternalOrderId { get; init; }

    [JsonPropertyName("oid")]
    public required string Oid { get; init; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("order")]
    public required HostedPageOrderStatus Order { get; init; }
}
