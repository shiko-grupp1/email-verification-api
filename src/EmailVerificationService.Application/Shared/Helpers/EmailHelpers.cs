using System.Security.Cryptography;

namespace EmailVerificationService.Application.Shared.Validators;

public static class EmailHelpers
{
    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
    public static string CreateVerificationCode()
    {
        int value = RandomNumberGenerator.GetInt32(1, 1_000_000);

        return value.ToString("D6");
    }
}
