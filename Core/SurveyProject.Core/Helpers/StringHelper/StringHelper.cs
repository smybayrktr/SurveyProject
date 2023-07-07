using System.Text.RegularExpressions;

namespace SurveyProject.Core.Helpers.StringHelper;

public static class StringHelper
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string RemoveHtml(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            return "";
        }
        String result = Regex.Replace(text, @"<[^>]*>", String.Empty);
        if (String.IsNullOrEmpty(result))
        {
            return "";
        }
        if (result.Length >= 30)
        {
            result = result.Substring(0, 30) + "...";
        }
        return result;
    }
    public static string GenerateRandomPassword(int length=12)
    {
        var random = new Random();
        var password = new char[length];

        for (int i = 0; i < password.Length; i++)
        {
            password[i] = chars[random.Next(chars.Length)];
        }

        return new string(password);
    }
}