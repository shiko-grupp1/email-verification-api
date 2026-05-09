namespace EmailVerificationService.Application.Shared.Results;
// INTE COMMITAD
public sealed record EmailVerificationResult
(
    bool IsVerified,
    string? Email = null,
    DateTimeOffset? IssuedAtUtc = null,
    DateTimeOffset? ExpiresAtUtc = null,
    string? Error = null
)
{
    public static EmailVerificationResult Success(string email, DateTimeOffset issuedAtUtc, DateTimeOffset expiresAtUtc) 
        => new(true, email,issuedAtUtc, expiresAtUtc);
    public static EmailVerificationResult Failure(string errorMessage) => new(false, Error: errorMessage);

}
