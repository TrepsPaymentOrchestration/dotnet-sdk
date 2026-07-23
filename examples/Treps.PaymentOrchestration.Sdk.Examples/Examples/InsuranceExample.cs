using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- insurance
///
/// Demonstrates an insurance-sector payment. Note this is the only insurance-specific endpoint —
/// voiding or refunding an insurance payment uses treps.Payments.VoidAsync()/RefundAsync() exactly
/// like any other transaction (see QuickstartExample).
/// </summary>
internal static class InsuranceExample
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
            var result = await treps.Insurance.PayAsync(new InsurancePaymentRequest
            {
                ExternalOrderId = $"example-insurance-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                Amount = 800.5m,
                Currency = "TRY",
                Installment = 1,
                ClientIp = "127.0.0.1",
                IsMoto = true,
                CardInsurance = new InsuranceCard
                {
                    CardOwnerName = "Mehmet Yılmaz",
                    CardBin = "12345678",
                    CardLastFour = "9876",
                    OwnerVknTckn = "1234567890",
                    CardOwnerCustomerId = "CUS_EXAMPLE_INS",
                },
            });

            Console.WriteLine($"Insurance payment result: {result.PaymentStatusMessage} {result.TransactionId}");

            // Refunding/voiding uses the exact same endpoints as a regular payment:
            var refund = await treps.Payments.RefundAsync(new RefundRequest
            {
                TransactionId = result.TransactionId,
                ExternalTransactionId = $"{result.ExternalTransactionId}-refund",
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
