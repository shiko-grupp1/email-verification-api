using EmailVerificationService.Api.Requests;
using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Application.Inputs;
using EmailVerificationService.Application.Shared.Results;

namespace EmailVerificationService.Api.Endpoints;

public static class EmailVerificationEndpoints
{
    public static void MapEmailVerificationEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/verification")
            .WithTags("Email Verification")
            .WithDescription("Endpoints for verifying emails");

        // Provides description for the specific endpoint, which will be used in the OpenAPI documentation
        group.MapPost("/request", RequestEmailVerificationEndpoint)
            .WithName("RequestEmailVerification")
            .WithSummary("Request email verification")
            .WithDescription("Generates a verification code and sends a verification email request to Azure Service Bus.")
            .Produces<EmailVerificationResult>(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status401Unauthorized);

    }

    private static async Task<IResult> RequestEmailVerificationEndpoint(IEmailVerificationService service, EmailVerificationRequest request, CancellationToken ct = default)
    {
        EmailVerificationInput input = new(request.Email);

        EmailVerificationResult result = await service.RequestEmailVerificationAsync(input, ct);

        return result.IsVerified
            ? Results.Accepted()
            : Results.BadRequest(result.Error);
    }
}