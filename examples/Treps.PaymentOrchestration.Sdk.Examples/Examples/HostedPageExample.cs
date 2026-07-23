using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- hosted-page
///
/// Demonstrates creating a Secure Payment Page (Hosted Page) session and then polling its
/// status by token — an alternative (or complement) to relying solely on the return_url callback.
/// </summary>
internal static class HostedPageExample
{
    public static async Task RunAsync()
    {
        using var treps = new TrepsClient(
            username: Environment.GetEnvironmentVariable("TREPS_USERNAME") ?? "",
            password: Environment.GetEnvironmentVariable("TREPS_PASSWORD") ?? "",
            merchantId: int.Parse(Environment.GetEnvironmentVariable("TREPS_MERCHANT_ID") ?? "0"),
            environment: TrepsEnvironment.Sandbox);

        var session = await treps.HostedPage.CreateAsync(new HostedPageInitRequest
        {
            ExternalOrderId = $"example-hpp-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            Amount = 2500,
            Currency = "TRY",
            TransactionType = 1,
            ReturnUrl = "https://example.com/payment/return",
            MinInstallment = 1,
            ExpireDate = DateTimeOffset.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
            CustomerCommissionPlanCode = "",
            Lang = "tr",
            ReturnButtonText = "Return to store",
            ReturnButtonUrl = "https://example.com",
            RedirectTimeout = 5,
        });

        Console.WriteLine($"Redirect the customer to: {session.Url}");
        Console.WriteLine($"Session token (for polling): {session.Token}");

        var status = await treps.HostedPage.QueryAsync(session.Token);
        Console.WriteLine($"Order completed? {status.Order.OrderCompleted}");
    }
}
