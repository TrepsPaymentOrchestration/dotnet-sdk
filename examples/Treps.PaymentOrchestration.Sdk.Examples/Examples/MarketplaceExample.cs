using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- marketplace
///
/// Demonstrates the Marketplace / Split Payment surface: registering a sub-merchant, approving
/// and allocating a paid-out order across sub-merchants, and settlement summary reporting.
/// </summary>
internal static class MarketplaceExample
{
    public static async Task RunAsync()
    {
        using var treps = new TrepsClient(
            username: Environment.GetEnvironmentVariable("TREPS_USERNAME") ?? "",
            password: Environment.GetEnvironmentVariable("TREPS_PASSWORD") ?? "",
            merchantId: int.Parse(Environment.GetEnvironmentVariable("TREPS_MERCHANT_ID") ?? "0"),
            environment: TrepsEnvironment.Sandbox);

        try
        {
            var subMerchantRefId = $"example-sub-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            var subMerchant = await treps.Marketplace.SubMerchants.AddAsync(new SubMerchantAddRequest
            {
                ReferenceId = subMerchantRefId,
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
            Console.WriteLine($"Sub-merchant created: {subMerchant.ReferenceId} ({subMerchant.SolePropFlagDesc})");

            var found = await treps.Marketplace.SubMerchants.FindAsync(new SubMerchantFindRequest { Status = 1, Page = 1, PageSize = 10 });
            Console.WriteLine($"Active sub-merchants: {found.TotalCount}");

            // A split-payment sale itself uses the existing treps.Payments.SaleAsync() with
            // SaleRequest.SubMerchants (see Models/Common.cs) — not shown here. Once such an
            // order exists, its per-sub-merchant allocation is approved/paid via:
            var oid = "ORDER-EXAMPLE-OID"; // the oid returned by the split-payment sale

            await treps.Marketplace.Order.ApproveAsync(
            [
                new OrderApproveItem { Oid = oid, SubMerchantReferenceId = subMerchantRefId, PartialApprove = false, ApproveAmount = 100m },
            ]);
            Console.WriteLine("Order approved for sub-merchant.");

            var allocateResult = await treps.Marketplace.Order.AllocatePayAsync(
            [
                new OrderPayAllocateItem { SubMerchantReferenceId = subMerchantRefId, Amount = 100m, PaymentReferenceCodes = ["PAYOUT-REF-1"] },
            ]);

            // Atomic: a per-item Success == true only means something if the top-level
            // Success is also true — otherwise the whole batch was rolled back.
            if (allocateResult.Success)
            {
                Console.WriteLine($"Allocation applied: {allocateResult.Items.Count} sub-merchant(s)");
            }
            else
            {
                Console.WriteLine($"Allocation rolled back entirely: {allocateResult.Message}");
            }

            var summary = await treps.Marketplace.Settlement.SummaryAsync(new SettlementSummaryRequest
            {
                SubMerchantReferenceIds = [subMerchantRefId],
            });
            foreach (var item in summary.Items)
            {
                Console.WriteLine($"Settlement for {item.SubMerchantReferenceId}: net={item.NetBalance} earned={item.EarnedBalance}");
            }

            // Export + async download job flow:
            await treps.Marketplace.Settlement.SummaryExportAsync(new SettlementSummaryExportRequest
            {
                ReportName = "example-settlement-export",
                Filter = new SettlementSummaryExportFilter { SubMerchantReferenceIds = [subMerchantRefId] },
            });

            var jobs = await treps.DownloadJobs.SearchAsync(new DownloadJobSearchRequest { ReportType = 1 /* SubMerchantSettlement */ });
            var completedJob = jobs.Items.FirstOrDefault(j => j.JobStatus == 2 /* Completed */);
            if (completedJob is not null)
            {
                var file = await treps.DownloadJobs.DownloadAsync(completedJob.Id);
                Console.WriteLine($"Downloaded export: {file.Content.Length} bytes ({file.ContentType})");
            }
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
