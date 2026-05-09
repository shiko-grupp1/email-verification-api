namespace EmailVerificationService.Application.Inputs;

public sealed record VerifyEmailCodeInput(string Email, string Code);

