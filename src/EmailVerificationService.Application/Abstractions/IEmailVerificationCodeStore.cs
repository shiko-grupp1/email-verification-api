using EmailVerificationService.Contracts.Contracts;

namespace EmailVerificationService.Application.Abstractions;

public interface IEmailVerificationCodeStore
{
    Task<bool> CacheCodeAsync(EmailVerificationCodeEntry codeEntry, CancellationToken ct = default);
    Task RemoveCodeAsync(string email, CancellationToken ct = default);
    Task<EmailVerificationCodeEntry?> GetCodeAsync(string email, CancellationToken ct = default);
}
