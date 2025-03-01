using System.Text.RegularExpressions;

namespace Infrastructure.Common;

public class Helper
{
    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < 8)
            return false;

        var hasUpper = false;
        var hasLower = false;
        var hasNumber = false;

        foreach (var c in password)
        {
            if (char.IsDigit(c))
                hasNumber = true;
            else if (char.IsUpper(c))
                hasUpper = true;
            else if (char.IsLower(c))
                hasLower = true;
        }
        
        return hasNumber && hasUpper && hasLower;
    }
    public static bool IsEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase);
        return emailRegex.IsMatch(email);
    }
}