using EmailVerificationService.Application.Inputs;
using EmailVerificationService.Application.Shared.Results;

namespace EmailVerificationService.Application.Abstractions;

public interface IEmailVerificationService
{
    Task<EmailVerificationResult> RequestEmailVerificationAsync(EmailVerificationInput input, CancellationToken ct = default);
}
