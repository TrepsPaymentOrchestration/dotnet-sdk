using Treps.PaymentOrchestration.Sdk.Examples.Examples;

var demo = args.Length > 0 ? args[0] : "quickstart";

Task task = demo switch
{
    "quickstart" => QuickstartExample.RunAsync(),
    "three-d-secure" => ThreeDSecureExample.RunAsync(),
    "hosted-page" => HostedPageExample.RunAsync(),
    "iframe" => IframeExample.RunAsync(),
    "query" => QueryExample.RunAsync(),
    "card" => CardExample.RunAsync(),
    "payment-link" => PaymentLinkExample.RunAsync(),
    "insurance" => InsuranceExample.RunAsync(),
    _ => throw new ArgumentException(
        $"Unknown demo \"{demo}\". Choose one of: quickstart, three-d-secure, hosted-page, iframe, query, card, payment-link, insurance."),
};

await task;
