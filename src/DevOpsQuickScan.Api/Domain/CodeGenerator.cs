using System.Security.Cryptography;
using System.Text;

namespace DevOpsQuickScan.Api.Domain;

public static class CodeGenerator
{
    private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int CodeLength = 5;

    public static string GenerateCode()
    {
        var result = new StringBuilder(CodeLength);
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] buffer = new byte[1];

            while (result.Length < CodeLength)
            {
                rng.GetBytes(buffer);
                var num = buffer[0] % AllowedChars.Length;
                result.Append(AllowedChars[num]);
            }
        }

        return result.ToString();
    }
}