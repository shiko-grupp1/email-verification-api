using EmailVerificationService.Contracts.Contracts;

namespace EmailVerificationService.Application.Abstractions;

public interface IEmailVerificationCodeStore
{
    Task<bool> CacheCodeAsync(EmailVerificationCodeEntry codeEntry, CancellationToken ct = default);

}
