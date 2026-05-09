namespace EmailVerificationService.Api.Requests;
public sealed record VerifyEmailCodeRequest(string Email, string Code);
