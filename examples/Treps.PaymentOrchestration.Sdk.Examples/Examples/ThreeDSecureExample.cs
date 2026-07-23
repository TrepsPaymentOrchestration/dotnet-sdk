using System.Security.Cryptography;
using System.Text;
using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- three-d-secure
///
/// Demonstrates the full 3D Secure flow:
///   1. InitAsync()             — start the challenge against the sandbox environment
///   2. decode RedirectContent  — see the HTML form you'd render to send the browser to the bank
///   3. HandleReturnUrl()       — how to verify + finalize the callback your server receives afterwards
///
/// Step 3 doesn't require a live redirect to demonstrate — it's shown here against a
/// locally-crafted example payload so you can see Hash.VerifyReturnUrlHash succeed/fail without
/// needing a browser or a running server.
/// </summary>
internal static class ThreeDSecureExample
{
    private static async Task HandleReturnUrl(TrepsClient treps, Dictionary<string, object?> payload, string threeDSecurityKey)
    {
        var isValid = Hash.VerifyReturnUrlHash(payload, threeDSecurityKey);
        if (!isValid)
        {
            Console.Error.WriteLine("Hash verification FAILED — refusing to trust this callback.");
            return;
        }

        if ((string?)payload["threeD_status"] != "SUCCESS")
        {
            Console.WriteLine($"3D Secure challenge did not succeed: {payload["threeD_status"]}");
            return;
        }

        Console.WriteLine("Hash verified, 3D Secure succeeded — finalizing the charge...");

        var result = await treps.ThreeDSecure.CompleteAsync(new ThreeDSecureCompleteRequest
        {
            Oid = (string)payload["oid"]!,
            PaymentId = (string)payload["payment_id"]!,
            TransactionId = (string)payload["transaction_id"]!,
        });

        Console.WriteLine($"Payment finalized: {result.PaymentStatusMessage}");
    }

    public static async Task RunAsync()
    {
        using var treps = new TrepsClient(
            username: Environment.GetEnvironmentVariable("TREPS_USERNAME") ?? "",
            password: Environment.GetEnvironmentVariable("TREPS_PASSWORD") ?? "",
            merchantId: int.Parse(Environment.GetEnvironmentVariable("TREPS_MERCHANT_ID") ?? "0"),
            environment: TrepsEnvironment.Sandbox);

        try
        {
            var init = await treps.ThreeDSecure.InitAsync(new ThreeDSecureInitRequest
            {
                ExternalOrderId = $"example-3ds-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                Amount = 100,
                Currency = "TRY",
                Installment = 1,
                ClientIp = "127.0.0.1",
                ReturnUrl = "https://your-site.example.com/payment/return",
                Card = new Card
                {
                    CardOwnerName = "Mehmet Yılmaz",
                    CardNumber = "5401341234567891",
                    CardExpireYear = "28",
                    CardExpireMonth = "12",
                    CardCvv = "000",
                    CardOwnerCustomerId = "CUS_EXAMPLE_3DS",
                },
            });

            Console.WriteLine($"Order created: {init.Oid} {init.ExternalOrderId}");

            var formHtml = Encoding.UTF8.GetString(Convert.FromBase64String(init.RedirectContent));
            Console.WriteLine($"Decoded auto-submit form (render this in the customer's browser):\n{formHtml}");

            // --- The rest happens after the bank redirects the browser back to your return_url. ---
            // Demonstrated below against a locally-crafted example payload, since we don't have a
            // live browser/bank round-trip in this script.
            var threeDSecurityKey = Environment.GetEnvironmentVariable("TREPS_3D_SECURITY_KEY") ?? "demo-secret-key";
            var examplePayload = new Dictionary<string, object?>
            {
                ["threeD_status"] = "SUCCESS",
                ["oid"] = init.Oid,
                ["payment_id"] = "PAY-2-example",
                ["transaction_id"] = "TRX-2-example",
                ["external_order_id"] = init.ExternalOrderId,
                ["order_amount"] = "100.00",
                ["amount"] = "100.00",
                ["installment"] = "1",
                ["currency"] = "TRY",
                ["complete_required"] = "YES",
                ["duplicate_request"] = "NO",
                ["payment_status"] = 4,
                ["threeD_secure_type"] = "FULL",
                ["return_url"] = "https://your-site.example.com/payment/return",
            };

            // In production this hash comes from the bank; here we compute one so the demo is runnable.
            var excluded = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "hash", "encoding", "countdown" };
            var keys = examplePayload.Keys
                .Where(k => !excluded.Contains(k))
                .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
                .ToList();
            static string Escape(object? v) => (v?.ToString() ?? "").Replace("\\", "\\\\").Replace("|", "\\|");
            var input = string.Join('|', keys.Select(k => Escape(examplePayload[k]))) + "|" + Escape(threeDSecurityKey);
            var demoHash = Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes(input)));

            var payloadWithHash = new Dictionary<string, object?>(examplePayload) { ["hash"] = demoHash };
            await HandleReturnUrl(treps, payloadWithHash, threeDSecurityKey);
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
