namespace EmailVerificationService.Infrastructure.Options;

public sealed class AzureServiceBusOptions
{
    public string EmailQueueName { get; init; } = string.Empty;
}
