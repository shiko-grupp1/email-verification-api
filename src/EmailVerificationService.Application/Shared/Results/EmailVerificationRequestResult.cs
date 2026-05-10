namespace EmailVerificationService.Application.Shared.Results;

public sealed record EmailVerificationRequestResult
(
    bool IsSuccess,
    string? Error = null,
    string? Email = null,
    DateTimeOffset? IssuedAtUtc = null,
    DateTimeOffset? ExpiresAtUtc = null 
)
{
    public static EmailVerificationRequestResult Success(string email, DateTimeOffset issuedAtUtc, DateTimeOffset expiresAtUtc) 
        => new(true, Email: email, IssuedAtUtc: issuedAtUtc, ExpiresAtUtc: expiresAtUtc);
    public static EmailVerificationRequestResult Failure(string errorMessage) => new(false, Error: errorMessage);

}
