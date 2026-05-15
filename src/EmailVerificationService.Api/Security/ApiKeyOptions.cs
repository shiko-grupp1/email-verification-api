namespace EmailVerificationService.Api.Security;

public sealed class ApiKeyOptions
{
    public const string SectionName = "ApiKey";
    public string HeaderName { get; init; } = "ApiKey";
    public string Value { get; init; } = null!;
}
