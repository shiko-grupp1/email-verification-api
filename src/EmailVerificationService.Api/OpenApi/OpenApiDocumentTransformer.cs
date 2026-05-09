using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EmailVerificationService.Api.OpenApi;
// Provides an overall description of the API.
public sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Info.Title = "Email Verification Service API";
        document.Info.Version = "v1";
        document.Info.Description = """
        ## Introduction

        The Email Verification Service API handles email verification requests for the platform.

        The service is responsible for:
        - generating verification codes
        - validating verification codes
        - checking if an email is verified
        - sending email requests to Azure Service Bus

        When a verification email needs to be sent, the service sends a message to Azure Service Bus. The Email Sender Function processes the message and sends the email through Azure Communication Services.

        """;

        return Task.CompletedTask;
    }
}
