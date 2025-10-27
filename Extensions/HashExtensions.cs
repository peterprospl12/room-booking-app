using System.Security.Cryptography;
using System.Text;

namespace Lab2.Extensions;

public static class HashExtensions
{
    public static string ToSha256Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyHashes(string password, string hash)
    {
        return ToSha256Hash(password) == hash;
    }
}
