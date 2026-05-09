using Azure.Messaging.ServiceBus;
using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Contracts.Contracts;
using EmailVerificationService.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EmailVerificationService.Infrastructure.Messaging;
// Sends verification email messages to Azure Service Bus for processing by the Email Sending service
public sealed class ServiceBusVerificationEmailPublisher(ServiceBusClient serviceBusClient, IOptions<AzureServiceBusOptions> options) : IVerificationEmailPublisher
{
    // Hämtar konfigurationen för Azure Service Bus från appsettings.json via IOptions-wrappern
    private readonly AzureServiceBusOptions _options = options.Value;
    public async Task PublishVerificationEmailAsync(VerificationEmailMessage message, CancellationToken ct = default)
    {
        string queueName = _options.EmailQueueName
            ?? throw new InvalidOperationException("Email queue name is missing.");

        // skapar ett objekt som kan skicka till kön i Azure Service Bus
        ServiceBusSender sender = serviceBusClient.CreateSender(queueName);

        string body = JsonSerializer.Serialize(message);

        // teknisk metadata för transport/spårning
        ServiceBusMessage serviceBusMessage = new(body)
        {
            ContentType = "application/json",
            CorrelationId = message.CorrelationId ?? message.To
        };

        // extra metadata
        serviceBusMessage.ApplicationProperties["Recipient"] = message.To;


        await sender.SendMessageAsync(serviceBusMessage, ct);
    }
}
