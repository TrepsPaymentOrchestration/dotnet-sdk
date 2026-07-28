using System.Net;
using System.Text.Json;
using Treps.PaymentOrchestration.Sdk.Models;
using Xunit;

namespace Treps.PaymentOrchestration.Sdk.Tests;

public class MarketplaceTests
{
    private static (TrepsClient Client, FakeHttpMessageHandler Handler) MakeClient()
    {
        var handler = new FakeHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var client = new TrepsClient(
            username: "user",
            password: "pass",
            merchantId: 1,
            baseUrl: "https://poapi.treps.tr",
            httpClient: httpClient);

        return (client, handler);
    }

    private static string LoginBody() =>
        $$"""
        {"status":true,"message":null,"data":{"access_token":"token-1","expire_in":{{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 60_000}},"scheme":"Bearer","token_policy":"apiuser"},"errors":null}
        """;

    [Fact]
    public async Task SubMerchantAddSendsPascalFieldsAsSnakeCaseJson()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody());
        handler.Enqueue(HttpStatusCode.OK, """
            {"status":true,"message":null,"data":{
                "reference_id":"SUB-1","name":"Acme","commercial_name":null,"sole_prop_flag":1,
                "sole_prop_flag_desc":"Sole Proprietor","tax_office":"Kadikoy","vkn_tckn":"1234567890",
                "address":"Addr","district":"Kadikoy","province_code":"34","country_alpha3":"TUR",
                "email":"a@b.com","phone":"5551112233","accounting_transfer_method":1,
                "accounting_transfer_method_desc":"IBAN","iban_owner_name":"Acme","iban":"TR00",
                "wallet_account_code":null,"contact_name":"John","contact_surname":"Doe",
                "blocked_day_count":7,"status":1,"insert_date":"2026-07-28T10:00:00Z"
            },"errors":null}
            """);

        var result = await client.Marketplace.SubMerchants.AddAsync(new SubMerchantAddRequest
        {
            ReferenceId = "SUB-1",
            Name = "Acme",
            SolePropFlag = 1,
            TaxOffice = "Kadikoy",
            VknTckn = "1234567890",
            Address = "Addr",
            District = "Kadikoy",
            ProvinceCode = "34",
            CountryAlpha3 = "TUR",
            Email = "a@b.com",
            Phone = "5551112233",
            AccountingTransferMethod = 1,
            IbanOwnerName = "Acme",
            Iban = "TR00",
            ContactName = "John",
            ContactSurname = "Doe",
            BlockedDayCount = 7,
            Status = 1,
        });

        Assert.Equal("SUB-1", result.ReferenceId);
        Assert.Equal("IBAN", result.AccountingTransferMethodDesc);

        var sentBody = handler.LastRequestBody!;
        using var doc = JsonDocument.Parse(sentBody);
        Assert.Equal("SUB-1", doc.RootElement.GetProperty("reference_id").GetString());
        Assert.Equal("1234567890", doc.RootElement.GetProperty("vkn_tckn").GetString());
        Assert.False(doc.RootElement.TryGetProperty("commercial_name", out _)); // null fields omitted
    }

    [Fact]
    public async Task OrderApproveSendsARawJsonArrayAsTheRequestBody()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody());
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":null,"errors":null}""");

        await client.Marketplace.Order.ApproveAsync(
        [
            new OrderApproveItem { Oid = "OID-1", SubMerchantReferenceId = "SUB-1", PartialApprove = false, ApproveAmount = 100m },
            new OrderApproveItem { Oid = "OID-2", SubMerchantReferenceId = "SUB-2", PartialApprove = true, ApproveAmount = 50m },
        ]);

        var sentBody = handler.LastRequestBody!;
        using var doc = JsonDocument.Parse(sentBody);
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
        Assert.Equal(2, doc.RootElement.GetArrayLength());
        Assert.Equal("OID-1", doc.RootElement[0].GetProperty("oid").GetString());
        Assert.Equal(100, doc.RootElement[0].GetProperty("approve_amount").GetDecimal());
    }

    [Fact]
    public async Task AllocatePayResponseSurfacesTheTopLevelAtomicSuccessFlagSeparatelyFromPerItemFlags()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody());
        handler.Enqueue(HttpStatusCode.OK, """
            {"status":true,"message":null,"data":{
                "success":false,
                "message":"insufficient balance for SUB-2",
                "items":[
                    {"sub_merchant_reference_id":"SUB-1","success":true,"error":null,"allocations":[{"oid":"OID-1","applied_amount":100}]},
                    {"sub_merchant_reference_id":"SUB-2","success":false,"error":"insufficient balance","allocations":[]}
                ]
            },"errors":null}
            """);

        var result = await client.Marketplace.Order.AllocatePayAsync(
        [
            new OrderPayAllocateItem { SubMerchantReferenceId = "SUB-1", Amount = 100m, PaymentReferenceCodes = ["REF-1"] },
            new OrderPayAllocateItem { SubMerchantReferenceId = "SUB-2", Amount = 50m, PaymentReferenceCodes = ["REF-2"] },
        ]);

        // The batch is atomic: even though items[0].Success == true, nothing was actually
        // applied because the top-level Success is false. Callers must check both.
        Assert.False(result.Success);
        Assert.True(result.Items[0].Success);
        Assert.False(result.Items[1].Success);
    }

    [Fact]
    public async Task DownloadJobDownloadReturnsRawBytesWithoutParsingTheApiResponseEnvelope()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody());
        handler.EnqueueRaw(HttpStatusCode.OK, "id,amount\nSUB-1,100"u8.ToArray(), "text/csv");

        var file = await client.DownloadJobs.DownloadAsync(42);

        Assert.Equal("id,amount\nSUB-1,100", System.Text.Encoding.UTF8.GetString(file.Content));
        Assert.Contains("text/csv", file.ContentType);
        Assert.Equal("GET", handler.Requests[^1].Method.ToString());
        Assert.EndsWith("/api/downloadjob/42/download", handler.Requests[^1].Url);
    }

    [Fact]
    public async Task DownloadJobCancelSendsADeleteRequest()
    {
        var (client, handler) = MakeClient();
        handler.Enqueue(HttpStatusCode.OK, LoginBody());
        handler.Enqueue(HttpStatusCode.OK, """{"status":true,"message":null,"data":null,"errors":null}""");

        await client.DownloadJobs.CancelAsync(42);

        Assert.Equal(HttpMethod.Delete, handler.Requests[^1].Method);
        Assert.EndsWith("/api/downloadjob/42/cancel", handler.Requests[^1].Url);
    }
}
