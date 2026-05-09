namespace EmailVerificationService.Application.Shared.Results;
public sealed record VerifyEmailCodeResult
(
    bool IsSuccess,
    bool IsVerified = false,
    string? Email = null,
    string? Error = null
)
{
    public static VerifyEmailCodeResult Success(string email)
        => new(true, true, email);
    public static VerifyEmailCodeResult Failure(string errorMessage) 
        => new(false, false, Error: errorMessage);

}