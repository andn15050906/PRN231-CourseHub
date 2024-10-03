using System.Security.Cryptography;
using System.Text;

namespace CourseHub.Core.Helpers.Cryptography;

/// <summary>
/// Needed in Infrastructure to perform DbContext's User.Password seeding
/// </summary>
internal static class SHA256Helper
{
    internal static string Hash(string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
            builder.Append(bytes[i].ToString("x2"));
        return builder.ToString();
    }
}