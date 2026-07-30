# Treps.PaymentOrchestration.Sdk

Official .NET SDK for the **Treps Payment Orchestration Platform**.

[![CI](https://github.com/TrepsPaymentOrchestration/dotnet-sdk/actions/workflows/ci.yml/badge.svg)](https://github.com/TrepsPaymentOrchestration/dotnet-sdk/actions/workflows/ci.yml)
[![NuGet version](https://img.shields.io/nuget/v/Treps.PaymentOrchestration.Sdk.svg)](https://www.nuget.org/packages/Treps.PaymentOrchestration.Sdk)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

This v0.1 release covers the core integration flows: **Authentication**, **Financial
Transactions** (sale / pre-auth / post-auth / refund / void), **3D Secure**, the
**Secure Payment Page** (Hosted Page) and its **embedded IFrame** variant, **Query
Operations** (BIN lookup, transaction/order detail & reporting, commissions, installments,
saved-card search), **Card Operations** (add / update / remove a tokenized card), **Payment
Links**, **Insurance-sector Payments**, and **Marketplace / Split Payments** (sub-merchants,
order approve/pay/allocate/seize/refund/collect/cancel, settlement reporting, and async report
export/download). See [Roadmap](#roadmap) for what's next.

Full API reference: **[REPLACE ME: public docs site URL]** (Transaction Types, 3D Secure,
Secure Payment Page, and Hash Verification pages).

## Install

```bash
dotnet add package Treps.PaymentOrchestration.Sdk
```

Requires .NET 8 or newer (uses the built-in `HttpClient` and `System.Text.Json` — zero NuGet
runtime dependencies).

## Quickstart

```csharp
using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Models;

using var treps = new TrepsClient(
    username: Environment.GetEnvironmentVariable("TREPS_USERNAME")!,
    password: Environment.GetEnvironmentVariable("TREPS_PASSWORD")!,
    merchantId: int.Parse(Environment.GetEnvironmentVariable("TREPS_MERCHANT_ID")!),
    environment: TrepsEnvironment.Sandbox); // or TrepsEnvironment.Production

var result = await treps.Payments.SaleAsync(new SaleRequest
{
    ExternalOrderId = "ORDER-1001",
    Amount = 800.5m,
    Currency = "TRY",
    Installment = 1,
    ClientIp = "192.168.1.105",
    Card = new Card
    {
        CardOwnerName = "Mehmet Evirgen",
        CardNumber = "5401341234567891",
        CardExpireYear = "28",
        CardExpireMonth = "12",
        CardCvv = "000",
        CardOwnerCustomerId = "CUS_78945612",
    },
});

Console.WriteLine($"{result.PaymentStatus} {result.PaymentStatusMessage}");
```

The client authenticates lazily (on first request) and caches the token, re-authenticating
automatically once it expires — you never call `LoginAsync()` yourself unless you want to warm
the cache up front.

Every request/response is a real C# `record` — `required` properties mirror the API's mandatory
fields, so a missing one is a compile error, not a runtime surprise.

## Financial transactions

```csharp
await treps.Payments.SaleAsync(new SaleRequest { /* ... */ });        // direct charge
await treps.Payments.PreAuthAsync(new PreAuthRequest { /* ... */ });  // reserve funds
await treps.Payments.PostAuthAsync(new PostAuthRequest { TransactionId = id, ExternalTransactionId = extId, ClientIp = ip });  // capture
await treps.Payments.RefundAsync(new RefundRequest { TransactionId = id, ExternalTransactionId = extId, ClientIp = ip });      // full/partial refund
await treps.Payments.VoidAsync(new VoidRequest { TransactionId = id, ExternalTransactionId = extId, ClientIp = ip });          // full cancellation
```

`PostAuthAsync`, `RefundAsync`, and `VoidAsync` each accept either `PaymentId` or
`TransactionId` to identify the original transaction. Note these three use `ClientIp`
(matching the wire field `clientIp`, camelCase); every other request shape uses the wire field
`client_ip` (snake_case) — a real asymmetry in the underlying API, not a typo.

## 3D Secure

```csharp
var init = await treps.ThreeDSecure.InitAsync(new ThreeDSecureInitRequest
{
    ExternalOrderId = "ORDER-1002",
    Amount = 100,
    Currency = "TRY",
    Installment = 1,
    ClientIp = clientIp,
    ReturnUrl = "https://your-site.com/payment/return",
    Card = new Card { /* ... */ },
});

// init.RedirectContent is a base64-encoded HTML auto-submit form.
// Decode it and render it to redirect the customer's browser to their bank.
```

After the bank redirects back to your `return_url`, **verify the callback before trusting
it**, then complete the charge:

```csharp
using Treps.PaymentOrchestration.Sdk;

// In your return_url route handler, build a Dictionary<string, object?> from the posted form/query fields.
var isValid = Hash.VerifyReturnUrlHash(returnUrlFields, Environment.GetEnvironmentVariable("TREPS_3D_SECURITY_KEY")!);
if (!isValid || (string?)returnUrlFields["threeD_status"] != "SUCCESS")
{
    return Results.Redirect("/payment/failed");
}

var result = await treps.ThreeDSecure.CompleteAsync(new ThreeDSecureCompleteRequest
{
    Oid = (string)returnUrlFields["oid"]!,
    PaymentId = (string)returnUrlFields["payment_id"]!,
    TransactionId = (string)returnUrlFields["transaction_id"]!,
});

// finalize the order using result
```

> **Never** trust `threeD_status == "SUCCESS"` without a passing `Hash.VerifyReturnUrlHash()`
> check first — the callback is a plain HTTP POST from the customer's browser and can be
> forged without it.

## Secure Payment Page (Hosted Page)

Let Treps host the entire payment form — redirect the customer to a URL instead of
collecting card details yourself:

```csharp
var session = await treps.HostedPage.CreateAsync(new HostedPageInitRequest
{
    ExternalOrderId = "ORDER-1003",
    Amount = 2500,
    Currency = "TRY",
    TransactionType = 1,
    ReturnUrl = "https://your-site.com/payment/return",
    MinInstallment = 1,
    ExpireDate = "2026-12-31T23:59:59Z",
    CustomerCommissionPlanCode = "",
    Lang = "tr",
    ReturnButtonText = "Return to store",
    ReturnButtonUrl = "https://your-site.com",
    RedirectTimeout = 5,
});

// redirect the customer to session.Url
```

The result is POSTed to your `return_url` the same way as the 3D Secure flow above (verify
with `Hash.VerifyReturnUrlHash()` first). You can also poll a session's status directly:

```csharp
var status = await treps.HostedPage.QueryAsync(session.Token);
Console.WriteLine($"{status.Order.OrderCompleted} {status.Order.OrderSuccessAmount}");
```

### Embedded IFrame checkout

Same endpoint, `iframe_flag: 1` — embed the returned URL in an `<iframe>` on your own page
instead of redirecting to it, with optional inline styling:

```csharp
var session = await treps.HostedPage.CreateIframeAsync(new IFrameInitRequest
{
    ExternalOrderId = "ORDER-1004",
    Amount = 2500,
    Currency = "TRY",
    TransactionType = 1,
    ReturnUrl = "https://your-site.com/payment/return",
    IframeWebUri = "https://your-site.com/checkout",
    CssVariables = new IFrameCssVariables
    {
        ButtonBackgroundColor = "#10b981",
        FontFamily = "Segoe UI, Roboto, sans-serif",
    },
});

// <iframe src="@session.Url"> on your checkout page
```

`QueryAsync(session.Token)` works the same way for both `CreateAsync()` and
`CreateIframeAsync()` sessions — it's the same underlying endpoint.

## Query operations

Read-only lookups and reporting — none of these trigger webhooks or change any state:

```csharp
// BIN lookup
var bin = await treps.Query.BinAsync(new BinQueryRequest { CardBin = "43550843" });

// Transaction / order detail
var tx = await treps.Query.TransactionDetailAsync(new TransactionDetailQueryRequest { TransactionId = "TRX-2-Tf8a6M5YtK" });
var order = await treps.Query.OrderDetailAsync(new OrderDetailQueryRequest { ExternalOrderId = "ORDER-1001" });

// Paginated reports
var transactions = await treps.Query.TransactionReportAsync(new TransactionReportQueryRequest { Page = 1, PageSize = 100 });
var orders = await treps.Query.OrderReportAsync(new OrderReportQueryRequest { Page = 1, PageSize = 100 });

// Commissions
var schemes = await treps.Query.CustomerCommissionsAsync();
var scheme = await treps.Query.CustomerCommissionItemsAsync(schemes[0].Code);

// Installment options for a card BIN + plan
var installments = await treps.Query.InstallmentsAsync(new InstallmentQueryRequest
{
    Bin = "48248929",
    Currency = "TRY",
    PlanCode = "Mus_01",
});

// Search saved/tokenized cards
var cards = await treps.Query.CardsAsync(new CardQueryRequest { CustomerCode = "CARD10" });
```

## Card operations

Save, update, and remove tokenized cards (searching them is `treps.Query.CardsAsync()` above,
since it's read-only):

```csharp
var card = await treps.Cards.AddAsync(new AddCardRequest
{
    CardOwnerName = "Mehmet Yılmaz",
    CustomerCode = "CARD10",
    CardNumber = "5406675406675403",
    CardExpireDate = "12/2040",
    CardAlias = "İş Bankası kartım",
    CardReferenceCode = "ref-001",
});

await treps.Cards.UpdateAsync(new UpdateCardRequest
{
    CustomerCode = "CARD10",
    CardToken = card.CardToken,
    CardAlias = "New alias",
    CardOwnerName = "Mehmet Yılmaz",
    CardExpireDate = "12/2040",
});

await treps.Cards.RemoveAsync(new RemoveCardRequest { CustomerCode = "CARD10", CardToken = card.CardToken });
```

## Payment links

Create a shareable link (email/SMS/WhatsApp) that the customer opens to complete payment on a
Treps-hosted page — no card details ever touch your servers:

```csharp
var link = await treps.PaymentLinks.CreateAsync(new PaymentLinkCreateRequest
{
    Name = "Invoice #1042",
    ExternalOrderId = "ORDER-1005",
    CustomerCommissionPlanCode = "",
    Amount = 2500,
    Currency = "TRY",
    TransactionType = 1,
    MinInstallment = 1,
    ExpireDate = "2026-12-31T23:59:59Z",
    OnetimeFlag = 1, // single-use: can't be paid again once completed
});

Console.WriteLine(link.Token); // share a URL built around this token, or query/list by it
```

```csharp
var details = await treps.PaymentLinks.GetAsync(new PaymentLinkQueryRequest { Token = link.Token });
Console.WriteLine($"{details.HaveCompletedOrder} {details.Status}");

var links = await treps.PaymentLinks.ListAsync(new PaymentLinkListRequest { Status = 1 });
```

> **Note:** the docs describe `status` inconsistently (a reference panel says `1`=active/`2`=expired,
> but real examples only ever show `0`/`1`). Prefer `HaveCompletedOrder` and `ExpireDate` over
> `Status` for the checks that actually matter to you.

## Insurance-sector payments

The only endpoint with an insurance-specific request shape is the payment itself — it takes a
tokenized `CardInsurance` reference (BIN + last four + the cardholder's tax ID) instead of raw
card details, and `IsMoto` is mandatory:

```csharp
var result = await treps.Insurance.PayAsync(new InsurancePaymentRequest
{
    ExternalOrderId = "ORDER-1006",
    Amount = 800.5m,
    Currency = "TRY",
    Installment = 1,
    ClientIp = clientIp,
    IsMoto = true,
    CardInsurance = new InsuranceCard
    {
        CardOwnerName = "Mehmet Yılmaz",
        CardBin = "12345678",
        CardLastFour = "9876",
        OwnerVknTckn = "1234567890",
        CardOwnerCustomerId = "CUS_78945612",
    },
});
```

Voiding or refunding an insurance payment uses the exact same `treps.Payments.VoidAsync()` /
`treps.Payments.RefundAsync()` methods as any other transaction — the docs site's "insurance
void/refund" entries are, at the wire level, identical to the regular ones (its "detached
refund" variant is in turn identical to its own regular refund entry, which looks like a
documentation copy/paste rather than an intentionally distinct contract), so nothing
insurance-specific is duplicated here.

## Marketplace / Split Payments

Register sub-merchants, then approve/pay out (or seize/refund/collect against) their share of a
split-payment order, and report on settlement balances.

```csharp
var subMerchant = await treps.Marketplace.SubMerchants.AddAsync(new SubMerchantAddRequest
{
    ReferenceId = "SUB-1001",
    Name = "Example Sub-Merchant A.Ş.",
    SolePropFlag = 1,
    TaxOffice = "Kadıköy",
    VknTckn = "1234567890",
    Address = "Örnek Mah. Örnek Sk. No:1",
    District = "Kadıköy",
    ProvinceCode = "34",
    CountryAlpha3 = "TUR",
    Email = "submerchant@example.com",
    Phone = "5551112233",
    AccountingTransferMethod = 1, // IBAN
    IbanOwnerName = "Example Sub-Merchant A.Ş.",
    Iban = "TR000006701000000000000001",
    ContactName = "Ayşe",
    ContactSurname = "Yılmaz",
    BlockedDayCount = 7,
    Status = 1,
});

var found = await treps.Marketplace.SubMerchants.FindAsync(new SubMerchantFindRequest { Status = 1, Page = 1, PageSize = 10 });
```

A split-payment sale itself is a regular `treps.Payments.SaleAsync()` call with
`SaleRequest.SubMerchants` populated (see [Models/Common.cs](./src/Treps.PaymentOrchestration.Sdk/Models/Common.cs)).
Once such an order exists, approve and pay out each sub-merchant's share — `ApproveAsync` and
`PayAsync` are **batch** endpoints; each posts a JSON array directly, not an object:

```csharp
await treps.Marketplace.Order.ApproveAsync(
[
    new OrderApproveItem { Oid = oid, SubMerchantReferenceId = "SUB-1001", PartialApprove = false, ApproveAmount = 100m },
]);

var allocateResult = await treps.Marketplace.Order.AllocatePayAsync(
[
    new OrderPayAllocateItem { SubMerchantReferenceId = "SUB-1001", Amount = 100m, PaymentReferenceCodes = ["PAYOUT-REF-1"] },
]);
```

> **`AllocatePayAsync` is atomic.** If `allocateResult.Success` is `false`, **nothing** in the
> batch was actually applied — the whole thing is rolled back — even if some individual
> `allocateResult.Items[i].Success` values are `true`. A row only really succeeded when both its
> own item-level `Success` AND the top-level `Success` are `true`.

Other order operations (`SeizedAsync`, `RefundAsync`, `CollectDebtAsync`, `CancelAsync`) take a
single object, not a batch array. Note `treps.Marketplace.Order.RefundAsync()` (order-level,
per sub-merchant) is distinct from `treps.Payments.RefundAsync()` (the top-level payment refund,
which can itself carry a `SubMerchants` allocation list — see [Error handling](#error-handling) below
and `RefundRequest.SubMerchants` in code). The same distinction applies to
`treps.Marketplace.Order.CancelAsync()` versus `treps.Payments.VoidAsync()` (with its own
`SubMerchants` allocation list). Both order-level methods are valid, documented endpoints, but
the reference frontend (MerchantPanel) does not use them for its actual refund/cancel flows — it
refunds and voids marketplace orders exclusively through `Payments.RefundAsync()` /
`Payments.VoidAsync()` with a `SubMerchants` allocation. Prefer that path unless you specifically
need order-level, per-sub-merchant refund/cancel semantics.

Settlement reporting, and an async export/download flow for larger reports:

```csharp
var summary = await treps.Marketplace.Settlement.SummaryAsync(new SettlementSummaryRequest { SubMerchantReferenceIds = ["SUB-1001"] });
var detail = await treps.Marketplace.Settlement.DetailAsync(new SettlementDetailRequest { SubMerchantReferenceIds = ["SUB-1001"], StatusFilter = 2 /* Unpaid */ });

await treps.Marketplace.Settlement.SummaryExportAsync(new SettlementSummaryExportRequest
{
    ReportName = "settlement-export",
    Filter = new SettlementSummaryExportFilter { SubMerchantReferenceIds = ["SUB-1001"] },
});

var jobs = await treps.DownloadJobs.SearchAsync(new DownloadJobSearchRequest { ReportType = 1 /* SubMerchantSettlement */, JobStatus = 2 /* Completed */ });
if (jobs.Items.Count > 0)
{
    var file = await treps.DownloadJobs.DownloadAsync(jobs.Items[0].Id); // raw bytes, not the ApiResponse<T> envelope
}
```

Finally, `treps.Marketplace.ConfigAsync(new MarketplaceConfigRequest { PaymentTransferApproveRequired = 1 })`
sets the platform-wide "require manual approval before transfer" setting. **This is write-only**
— there is no GET/read endpoint for it on the backend as of this writing, so there's no way to
fetch the current value back.

## Error handling

Any failed request (non-2xx HTTP, or a 2xx response with `status: false`) throws
`Treps.PaymentOrchestration.Sdk.Exceptions.TrepsApiException`:

```csharp
using Treps.PaymentOrchestration.Sdk.Exceptions;

try
{
    await treps.Payments.SaleAsync(new SaleRequest { /* ... */ });
}
catch (TrepsApiException ex)
{
    Console.Error.WriteLine($"{ex.Message} {ex.HttpStatus}");
    if (ex.Errors is not null)
    {
        Console.Error.WriteLine(string.Join(", ", ex.Errors));
    }
    throw;
}
```

## Examples

Runnable, self-contained demos under [`examples/`](./examples), dispatched by name:

| Demo name | Demonstrates |
| --- | --- |
| `quickstart` | `Sale` followed by a `Refund` |
| `three-d-secure` | `ThreeDSecure.InitAsync`, decoding the redirect form, and verifying + completing the return_url callback |
| `hosted-page` | Creating a Secure Payment Page session and polling it by token |
| `iframe` | Embedded IFrame checkout with custom CSS variables |
| `query` | BIN lookup, transaction/order detail, reports, commissions, installments, and card search |
| `card` | Add, update, search, and remove a saved card |
| `payment-link` | Create a payment link, query its status, and list links |
| `insurance` | An insurance-sector payment, then refunding it via the regular refund endpoint |
| `marketplace` | Registering a sub-merchant, approving/allocating a split-payment order, settlement summary, and the async export/download job flow |

```bash
TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- quickstart
```

## Environments

| Environment | Base URL |
| --- | --- |
| `TrepsEnvironment.Sandbox` | `https://poapi.treps.tr` |
| `TrepsEnvironment.Production` | `https://api.treps.io` |

Pass a custom `baseUrl` instead of `environment` to point at any other host (e.g. a local mock
server in tests), or a custom `httpClient` to control the connection pool, timeouts, or supply
your own handler (e.g. `IHttpClientFactory` in ASP.NET Core apps).

## Roadmap

Not yet covered by this SDK (planned as fast-follow module):

- Closed Loop Wallet

Contributions and issues are welcome — see [SECURITY.md](./SECURITY.md) for reporting
vulnerabilities specifically.

## License

[MIT](./LICENSE)
