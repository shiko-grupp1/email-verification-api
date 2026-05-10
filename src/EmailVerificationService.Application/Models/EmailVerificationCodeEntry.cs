namespace EmailVerificationService.Contracts.Contracts;

// Provides temporary storage for email verification codes. Codes are stored when a verification email is requested
// and later retrieved when the user attempts to verify their email address.
public sealed record EmailVerificationCodeEntry
(
    string Email, 
    string Code, 
    DateTimeOffset IssuedAtUtc, 
    DateTimeOffset ExpiresAtUtc
);

