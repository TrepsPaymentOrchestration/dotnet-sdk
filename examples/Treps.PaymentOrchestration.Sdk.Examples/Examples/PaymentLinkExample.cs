using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- payment-link
///
/// Demonstrates creating a shareable payment link, querying its status, and listing links.
/// </summary>
internal static class PaymentLinkExample
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
            var link = await treps.PaymentLinks.CreateAsync(new PaymentLinkCreateRequest
            {
                Name = "Example invoice",
                ExternalOrderId = $"example-link-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                CustomerCommissionPlanCode = "",
                Amount = 2500,
                Currency = "TRY",
                TransactionType = 1,
                MinInstallment = 1,
                ExpireDate = DateTimeOffset.UtcNow.AddDays(30).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                // Single-use: the link can't be paid again once completed.
                OnetimeFlag = 1,
                Buyer = new PaymentLinkBuyer
                {
                    CustomerId = "CUST-EXAMPLE",
                    Name = "Mehmet",
                    Surname = "Yılmaz",
                    Email = "mehmet.yilmaz@example.com",
                    PhoneNumber = "5324567890",
                },
            });

            Console.WriteLine($"Share this link (token): {link.Token}");

            var details = await treps.PaymentLinks.GetAsync(new PaymentLinkQueryRequest { Token = link.Token });
            Console.WriteLine($"HaveCompletedOrder: {details.HaveCompletedOrder} | Status: {details.Status}");

            var list = await treps.PaymentLinks.ListAsync(new PaymentLinkListRequest { Token = link.Token });
            Console.WriteLine($"Payment links matching this token: {list.TotalCount} total");
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
