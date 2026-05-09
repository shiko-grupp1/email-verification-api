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

        EmailVerificationCodeEntry entry = new(normalizedEmail, verificationCode, issuedAtUtc, expiresAtUtc);

        bool isStored = await codeStore.CacheCodeAsync(entry, ct);
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

    public async Task<VerifyEmailCodeResult> VerifyEmailCodeAsync(VerifyEmailCodeInput input, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(input.Email))
            return VerifyEmailCodeResult.Failure("Email address is required.");

        if (string.IsNullOrWhiteSpace(input.Code))
            return VerifyEmailCodeResult.Failure("Verification code is required.");

        string normalizedEmail = EmailHelpers.NormalizeEmail(input.Email);
        string normalizedCode = input.Code.Trim();

        EmailVerificationCodeEntry? entry = await codeStore.GetCodeAsync(normalizedEmail, ct);
        if (entry is null)
            return VerifyEmailCodeResult.Failure("Verification code has expired or does not exist.");

        if (entry.ExpiresAtUtc <= DateTimeOffset.UtcNow)
        {
            await codeStore.RemoveCodeAsync(entry.Email, ct);

            return VerifyEmailCodeResult.Failure("Verification code has expired.");
        }

        if (entry.Code != normalizedCode)
            return VerifyEmailCodeResult.Failure("Invalid verification code.");


        await codeStore.RemoveCodeAsync(entry.Email, ct);
        return VerifyEmailCodeResult.Success(normalizedEmail);
    }
}
