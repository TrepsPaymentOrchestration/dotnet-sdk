using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- iframe
///
/// Demonstrates the embedded IFrame checkout — the same /api/payment/hostedpage endpoint as the
/// Secure Payment Page, but with IframeFlag: 1 and styling via CssVariables.
/// </summary>
internal static class IframeExample
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
            var session = await treps.HostedPage.CreateIframeAsync(new IFrameInitRequest
            {
                ExternalOrderId = $"example-iframe-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                Amount = 2500,
                Currency = "TRY",
                TransactionType = 1,
                ReturnUrl = "https://example.com/payment/return",
                IframeWebUri = "https://example.com/checkout",
                Lang = "tr",
                CssVariables = new IFrameCssVariables
                {
                    TextColor = "#1f2937",
                    FontFamily = "Segoe UI, Roboto, sans-serif",
                    FontSize = "16px",
                    ButtonBackgroundColor = "#10b981",
                    ButtonBackgroundColorHover = "#059669",
                    HideInstallments = "0",
                    HidePayButton = "0",
                },
            });

            Console.WriteLine($"Embed this in an <iframe src=\"...\">: {session.Url}");
            Console.WriteLine($"Session token (for polling): {session.Token}");

            // QueryAsync() works the same way for both CreateIframeAsync() and CreateAsync() (Hosted Page) sessions.
            var status = await treps.HostedPage.QueryAsync(session.Token);
            Console.WriteLine($"Order completed? {status.Order.OrderCompleted}");
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
