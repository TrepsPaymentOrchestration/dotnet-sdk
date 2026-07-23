using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- quickstart
///
/// Demonstrates a direct sale followed by a full refund against the sandbox environment.
/// </summary>
internal static class QuickstartExample
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
            var externalOrderId = $"example-order-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            var sale = await treps.Payments.SaleAsync(new SaleRequest
            {
                ExternalOrderId = externalOrderId,
                Amount = 10.5m,
                Currency = "TRY",
                Installment = 1,
                ClientIp = "127.0.0.1",
                Card = new Card
                {
                    CardOwnerName = "Mehmet Yılmaz",
                    CardNumber = "5401341234567891",
                    CardExpireYear = "28",
                    CardExpireMonth = "12",
                    CardCvv = "000",
                    CardOwnerCustomerId = "CUS_EXAMPLE_1",
                },
            });

            Console.WriteLine($"Sale result: {sale.PaymentStatusMessage} {sale.TransactionId}");

            var refund = await treps.Payments.RefundAsync(new RefundRequest
            {
                TransactionId = sale.TransactionId,
                ExternalTransactionId = $"{externalOrderId}-refund",
                ClientIp = "127.0.0.1",
            });

            Console.WriteLine($"Refund result: {refund.ResultMessage}");
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
