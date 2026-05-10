using EmailVerificationService.Contracts.Contracts;

namespace EmailVerificationService.Application.Abstractions;

// Publishes verification email messages (to servicebus) so another service can send the actual email.
public interface IVerificationEmailPublisher
{
    Task PublishVerificationEmailAsync(VerificationEmailMessage message, CancellationToken ct = default);
}
