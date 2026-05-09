using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Application.Inputs;
using EmailVerificationService.Application.Options;
using EmailVerificationService.Application.Shared.Results;
using EmailVerificationService.Application.Shared.Validators;
using EmailVerificationService.Contracts.Contracts;
using Microsoft.Extensions.Options;

namespace EmailVerificationService.Application.Services;

public sealed class EmailVerificationManager(IVerificationEmailPublisher emailPublisher, IEmailVerificationCodeStore codeStore, IOptions<EmailVerificationOptions> options) : IEmailVerificationService
{
    private readonly EmailVerificationOptions _options = options.Value;

    public async Task<EmailVerificationRequestResult> RequestEmailVerificationAsync(EmailVerificationInput input, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(input.Email))
            return EmailVerificationRequestResult.Failure("Email address is required.");

        string normalizedEmail = EmailHelpers.NormalizeEmail(input.Email);

        string verificationCode = EmailHelpers.CreateVerificationCode();

        DateTimeOffset issuedAtUtc = DateTimeOffset.UtcNow;

        int expiresInMinutes = _options.CodeExpirationMinutes;
        DateTimeOffset expiresAtUtc = issuedAtUtc.AddMinutes(expiresInMinutes);

        EmailVerificationCodeEntry codeEntry = new(normalizedEmail, verificationCode, issuedAtUtc, expiresAtUtc);

        bool isStored = await codeStore.CacheCodeAsync(codeEntry, ct);
        if (!isStored)
            return EmailVerificationRequestResult.Failure("Unable to store verification code");

        VerificationEmailMessage message = new(normalizedEmail, verificationCode, expiresInMinutes);

        await emailPublisher.PublishVerificationEmailAsync(message, ct);

        return EmailVerificationRequestResult.Success
        (
            normalizedEmail,
            issuedAtUtc,
            expiresAtUtc
        );

    }

}
