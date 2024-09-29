using System.Text;
using System.Text.RegularExpressions;

namespace CourseHub.Core.Helpers.Text;

internal class TextHelper
{
    internal static string Normalize(string original)
    {
        string normalized = original.Trim();

        // from Space to Slash in ASCII
        for (byte i = 32; i < 48; i++)
            normalized = normalized.Replace(((char)i).ToString(), " ");

        normalized = normalized.Normalize(NormalizationForm.FormD);

        Regex regex = new(@"\p{IsCombiningDiacriticalMarks}+");
        normalized = regex.Replace(normalized, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        while (normalized.Contains('?', StringComparison.CurrentCulture))
        {
            normalized = normalized.Remove(normalized.IndexOf("?"), 1);
        }

        return normalized.Replace("--", "-").ToLower();
    }

    internal static string GenerateAlphanumeric(byte length)
    {
        string charList = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        Random rd = new();
        StringBuilder builder = new();
        byte i;

        for (i = 0; i < 6; i++)
            builder.Append(charList[rd.Next(charList.Length)]);
        return builder.ToString();
    }
}
