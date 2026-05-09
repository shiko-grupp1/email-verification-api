namespace EmailVerificationService.Application.Options;

public sealed class EmailVerificationOptions
{
    public int CodeExpirationMinutes { get; init; }
}
