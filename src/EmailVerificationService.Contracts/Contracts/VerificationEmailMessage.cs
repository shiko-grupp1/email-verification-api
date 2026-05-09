namespace EmailVerificationService.Contracts.Contracts;

public sealed record VerificationEmailMessage(
    string To,
    string VerificationCode,
    int ExpiresInMinutes,
    string? CorrelationId = null
);

