using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- query
///
/// Demonstrates the read-only query/reporting operations: BIN lookup, transaction and order
/// detail, paginated reports, commission schemes, installment options, and saved-card search.
/// </summary>
internal static class QueryExample
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
            var bin = await treps.Query.BinAsync(new BinQueryRequest { CardBin = "43550843" });
            Console.WriteLine($"BIN lookup: {bin.CardBrand} {bin.CardNetwork} {bin.CardCountry}");

            var report = await treps.Query.TransactionReportAsync(new TransactionReportQueryRequest { Page = 1, PageSize = 10 });
            Console.WriteLine($"Transaction report: {report.Count} transaction(s) on this page");

            if (report.Count > 0)
            {
                var detail = await treps.Query.TransactionDetailAsync(new TransactionDetailQueryRequest { TransactionId = report[0].TransactionId });
                Console.WriteLine($"Transaction detail: {detail.TransactionId} {detail.ResultMessage ?? detail.TransactionStatus.ToString()}");
            }

            var orders = await treps.Query.OrderReportAsync(new OrderReportQueryRequest { Page = 1, PageSize = 10 });
            Console.WriteLine($"Order report: {orders.TotalCount} order(s) total");

            if (orders.Data.Count > 0)
            {
                var orderDetail = await treps.Query.OrderDetailAsync(new OrderDetailQueryRequest { ExternalOrderId = orders.Data[0].ExternalOrderId });
                Console.WriteLine($"Order detail products: {string.Join(", ", orderDetail.Order.Products.Select(p => p.Name))}");
            }

            var schemes = await treps.Query.CustomerCommissionsAsync();
            Console.WriteLine($"Commission schemes: {schemes.Count}");

            if (schemes.Count > 0)
            {
                var items = await treps.Query.CustomerCommissionItemsAsync(schemes[0].Code);
                Console.WriteLine($"Commission items for {schemes[0].Code}: {items.Items.Count}");
            }

            var installments = await treps.Query.InstallmentsAsync(new InstallmentQueryRequest
            {
                Bin = "48248929",
                Amount = 100,
                Currency = "TRY",
                PlanCode = "Mus_01",
            });
            Console.WriteLine($"Installment options: {installments.Installments.Count}");

            var cards = await treps.Query.CardsAsync(new CardQueryRequest { CustomerCode = "CARD10" });
            Console.WriteLine($"Saved cards for CARD10: {cards.Cards.Count}");
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
