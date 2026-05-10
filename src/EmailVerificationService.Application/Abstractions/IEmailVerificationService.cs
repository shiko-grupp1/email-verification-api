using EmailVerificationService.Application.Inputs;
using EmailVerificationService.Application.Shared.Results;

namespace EmailVerificationService.Application.Abstractions;

public interface IEmailVerificationService
{
    Task<EmailVerificationRequestResult> RequestEmailVerificationAsync(EmailVerificationInput input, CancellationToken ct = default);
    Task<VerifyEmailCodeResult> VerifyEmailCodeAsync(VerifyEmailCodeInput input, CancellationToken ct = default);
}
