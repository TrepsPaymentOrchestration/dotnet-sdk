using System.Security.Cryptography;
using System.Text;

namespace Treps.PaymentOrchestration.Sdk;

/// <summary>
/// Verifies the <c>hash</c> field on a payload posted to your <c>return_url</c> (by either the
/// 3D Secure or Hosted Page flow), proving it was not tampered with in transit.
/// </summary>
/// <remarks>
/// Algorithm (must match byte-for-byte, ported from the documented reference implementation):
/// 1. Drop <c>hash</c>, <c>encoding</c>, <c>countdown</c>, and any field with an empty value.
/// 2. Sort the remaining keys alphabetically, case-insensitive.
/// 3. Escape each value: <c>\</c> -&gt; <c>\\</c>, <c>|</c> -&gt; <c>\|</c> (same escaping applied to the secret key).
/// 4. Join the escaped values with <c>|</c>, then append the escaped secret key.
/// 5. SHA-512 the resulting string and Base64-encode the digest.
/// </remarks>
public static class Hash
{
    private static readonly HashSet<string> AlwaysExcluded = new(StringComparer.OrdinalIgnoreCase)
    {
        "hash",
        "encoding",
        "countdown",
    };

    private static string EscapeHashValue(object? value)
    {
        var stringValue = value?.ToString() ?? string.Empty;

        return stringValue.Replace("\\", "\\\\").Replace("|", "\\|");
    }

    /// <summary>
    /// Verifies a return_url payload against your 3D Security Key.
    /// </summary>
    /// <param name="payload">
    /// The raw fields exactly as received on your return_url endpoint — do not rename or remap
    /// keys/values before calling this.
    /// </param>
    /// <param name="secretKey">Your 3D Security Key, from the Treps Portal (Settings).</param>
    /// <returns><c>true</c> if the computed hash matches <c>payload["hash"]</c>.</returns>
    /// <remarks>Never trust <c>threeD_status == "SUCCESS"</c> without this check passing.</remarks>
    public static bool VerifyReturnUrlHash(IReadOnlyDictionary<string, object?> payload, string secretKey)
    {
        if (!payload.TryGetValue("hash", out var providedHashValue) || providedHashValue is not string providedHash || providedHash.Length == 0)
        {
            return false;
        }

        var sortedKeys = payload.Keys
            .Where(key => !AlwaysExcluded.Contains(key))
            .Where(key => payload[key] is not null && payload[key] as string != string.Empty)
            .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var hashInput = string.Join('|', sortedKeys.Select(key => EscapeHashValue(payload[key])))
            + "|" + EscapeHashValue(secretKey);

        var digest = SHA512.HashData(Encoding.UTF8.GetBytes(hashInput));
        var calculatedHash = Convert.ToBase64String(digest);

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(calculatedHash),
            Encoding.UTF8.GetBytes(providedHash));
    }
}
