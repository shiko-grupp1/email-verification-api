using EmailVerificationService.Contracts.Contracts;

namespace EmailVerificationService.Application.Abstractions;

public interface IVerificationEmailPublisher
{
    Task PublishVerificationEmailAsync(VerificationEmailMessage message, CancellationToken ct = default);
}
