using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Treps.PaymentOrchestration.Sdk.Tests;

public class HashTests
{
    private const string SecretKey = "test-3d-security-key";

    private static Dictionary<string, object?> BasePayload() => new()
    {
        ["threeD_status"] = "SUCCESS",
        ["oid"] = "ORD-2-s5R6Bm",
        ["payment_id"] = "PAY-2-o5MKk8e",
        ["transaction_id"] = "TRX-2-n4T1Fti5DE",
        ["amount"] = "301.77",
        ["currency"] = "TRY",
        ["installment"] = "1",
    };

    /// <summary>Independently re-implements the algorithm to compute a valid hash for test fixtures.</summary>
    private static string ComputeHash(Dictionary<string, object?> payload, string secretKey)
    {
        var excluded = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "hash", "encoding", "countdown" };
        var keys = payload.Keys
            .Where(k => !excluded.Contains(k))
            .Where(k => payload[k] is not null && payload[k] as string != string.Empty)
            .OrderBy(k => k.ToLowerInvariant(), StringComparer.Ordinal)
            .ToList();

        static string Escape(object? v) => (v?.ToString() ?? string.Empty).Replace("\\", "\\\\").Replace("|", "\\|");

        var input = string.Join('|', keys.Select(k => Escape(payload[k]))) + "|" + Escape(secretKey);
        var digest = SHA512.HashData(Encoding.UTF8.GetBytes(input));

        return Convert.ToBase64String(digest);
    }

    [Fact]
    public void ReturnsTrueForCorrectlyComputedHash()
    {
        var payload = BasePayload();
        var hash = ComputeHash(payload, SecretKey);
        payload["hash"] = hash;

        Assert.True(Hash.VerifyReturnUrlHash(payload, SecretKey));
    }

    [Fact]
    public void ReturnsFalseWhenFieldValueIsTampered()
    {
        var payload = BasePayload();
        var hash = ComputeHash(payload, SecretKey);
        var tampered = new Dictionary<string, object?>(payload) { ["amount"] = "999.99", ["hash"] = hash };

        Assert.False(Hash.VerifyReturnUrlHash(tampered, SecretKey));
    }

    [Fact]
    public void ReturnsFalseWhenSecretKeyIsWrong()
    {
        var payload = BasePayload();
        var hash = ComputeHash(payload, SecretKey);
        payload["hash"] = hash;

        Assert.False(Hash.VerifyReturnUrlHash(payload, "wrong-secret"));
    }

    [Fact]
    public void ReturnsFalseWhenHashIsMissingOrEmpty()
    {
        var withEmptyHash = BasePayload();
        withEmptyHash["hash"] = "";
        Assert.False(Hash.VerifyReturnUrlHash(withEmptyHash, SecretKey));

        Assert.False(Hash.VerifyReturnUrlHash(BasePayload(), SecretKey));
    }

    [Fact]
    public void IgnoresHashEncodingAndCountdownFields()
    {
        var payload = BasePayload();
        var hash = ComputeHash(payload, SecretKey);
        payload["hash"] = hash;
        payload["encoding"] = "utf-8";
        payload["countdown"] = "30";

        Assert.True(Hash.VerifyReturnUrlHash(payload, SecretKey));
    }

    [Fact]
    public void IgnoresFieldsWithEmptyStringValues()
    {
        var withEmpty = BasePayload();
        withEmpty["external_transaction_id"] = "";
        var hash = ComputeHash(withEmpty, SecretKey);
        var payload = new Dictionary<string, object?>(withEmpty) { ["hash"] = hash };

        Assert.True(Hash.VerifyReturnUrlHash(payload, SecretKey));

        var mutated = new Dictionary<string, object?>(payload) { ["external_transaction_id"] = "now-not-empty" };
        Assert.False(Hash.VerifyReturnUrlHash(mutated, SecretKey));
    }

    private static string Sha512Base64(string input) => Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes(input)));

    [Fact]
    public void MatchesDocumentedExampleInputString()
    {
        // Real Return URL payload and its hash input string from the docs (Hash Control page).
        var payload = new Dictionary<string, object?>
        {
            ["threeD_status"] = "SUCCESS",
            ["oid"] = "ORD-2-s5R6Bm",
            ["payment_id"] = "PAY-2-o5MKk8e",
            ["transaction_id"] = "TRX-2-n4T1Fti5DE",
            ["external_order_id"] = "9d436c3b-ae6f-48db-96dc-a492c183e525",
            ["external_transaction_id"] = "6f0175a1-c03d-4f15-913d-2d5050e6f564",
            ["order_amount"] = "301.77",
            ["amount"] = "301.77",
            ["card_amount"] = "0.00",
            ["interest_amount"] = "0.00",
            ["point_amount"] = "0.00",
            ["wallet_amount"] = "0.00",
            ["external_wallet_account_id"] = "",
            ["wallet_payment_id"] = "",
            ["wallet_cashback"] = "0.00",
            ["installment"] = "1",
            ["currency"] = "TRY",
            ["complete_required"] = "YES",
            ["duplicate_request"] = "NO",
            ["payment_status"] = "4",
            ["threeD_secure_type"] = "FULL",
            ["return_url"] = "https://webhook.site/a0a512d1-5cd2-4584-895e-e86e06bf17d0",
            ["retry_fail"] = "FALSE",
            ["retry_count"] = "0",
        };
        const string documentedInput =
            "301.77|0.00|YES|TRY|NO|9d436c3b-ae6f-48db-96dc-a492c183e525|6f0175a1-c03d-4f15-913d-2d5050e6f564|1|0.00|ORD-2-s5R6Bm|301.77|PAY-2-o5MKk8e|4|0.00|0|FALSE|https://webhook.site/a0a512d1-5cd2-4584-895e-e86e06bf17d0|FULL|SUCCESS|TRX-2-n4T1Fti5DE|0.00|0.00|";
        payload["hash"] = Sha512Base64(documentedInput + SecretKey);

        Assert.True(Hash.VerifyReturnUrlHash(payload, SecretKey));
    }

    [Fact]
    public void SortsLowercasedKeysByOrdinalCharCode()
    {
        // Ordinal on lowercased keys: '1' (0x31) < '_' (0x5F) < 'y' (0x79).
        // OrdinalIgnoreCase would give field1, fieldy, field_x (it compares upper-cased chars).
        var payload = new Dictionary<string, object?> { ["fieldy"] = "C", ["field_x"] = "B", ["field1"] = "A" };
        payload["hash"] = Sha512Base64("A|B|C|" + SecretKey);

        Assert.True(Hash.VerifyReturnUrlHash(payload, SecretKey));
    }

    [Fact]
    public void EscapesBackslashAndPipeCharactersInValues()
    {
        var payload = BasePayload();
        payload["return_url"] = "https://example.com/cb?a=1|2&b=x\\y";
        var hash = ComputeHash(payload, SecretKey);
        payload["hash"] = hash;

        Assert.True(Hash.VerifyReturnUrlHash(payload, SecretKey));
    }
}
