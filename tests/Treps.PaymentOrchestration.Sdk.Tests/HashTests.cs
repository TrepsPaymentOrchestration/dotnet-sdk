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
            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
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
