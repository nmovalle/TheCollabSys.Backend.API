using System.Text;

namespace TheCollabSys.Backend.API.Extensions;

public static class PasswordUtils
{
    public static string GeneratePassword(int length)
    {
        if (length < 1 || length > 10)
        {
            throw new ArgumentException("Length must be between 1 and 10.");
        }

        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{}|;:',.<>?";
        var random = new Random();
        var password = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            var index = random.Next(characters.Length);
            password.Append(characters[index]);
        }

        return password.ToString();
    }
}
