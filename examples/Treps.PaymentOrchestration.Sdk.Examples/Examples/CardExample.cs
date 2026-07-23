using Treps.PaymentOrchestration.Sdk;
using Treps.PaymentOrchestration.Sdk.Exceptions;
using Treps.PaymentOrchestration.Sdk.Models;

namespace Treps.PaymentOrchestration.Sdk.Examples.Examples;

/// <summary>
/// Run with: TREPS_USERNAME=... TREPS_PASSWORD=... TREPS_MERCHANT_ID=... dotnet run --project examples -- card
///
/// Demonstrates saved/tokenized card management: add, update, remove, and (via the query
/// resource) search.
/// </summary>
internal static class CardExample
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
            var customerCode = $"example-customer-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            var added = await treps.Cards.AddAsync(new AddCardRequest
            {
                CardOwnerName = "Mehmet Yılmaz",
                CustomerCode = customerCode,
                CardNumber = "5406675406675403",
                CardExpireDate = "12/2040",
                CardAlias = "İş Bankası kartım",
                CardReferenceCode = $"ref-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            });
            Console.WriteLine($"Card added, token: {added.CardToken}");

            await treps.Cards.UpdateAsync(new UpdateCardRequest
            {
                CustomerCode = customerCode,
                CardToken = added.CardToken,
                CardAlias = "Yeni takma ad",
                CardOwnerName = "Mehmet Yılmaz",
                CardExpireDate = "12/2040",
            });
            Console.WriteLine("Card alias updated");

            var found = await treps.Query.CardsAsync(new CardQueryRequest { CustomerCode = customerCode });
            Console.WriteLine($"Cards for {customerCode}: {found.Cards.Count}");

            var removed = await treps.Cards.RemoveAsync(new RemoveCardRequest { CustomerCode = customerCode, CardToken = added.CardToken });
            Console.WriteLine($"Card removed: {removed.Removed}");
        }
        catch (TrepsApiException ex)
        {
            Console.Error.WriteLine($"Treps API error: {ex.Message} {string.Join(", ", ex.Errors ?? [])}");
            Environment.ExitCode = 1;
        }
    }
}
