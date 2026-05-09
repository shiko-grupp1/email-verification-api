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
            .Produces<EmailVerificationRequestResult>(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest);
        

        group.MapPost("/verify", VerifyEmailCodeEndpoint)
            .WithName("VerifyEmail")
            .WithSummary("Verify email")
            .WithDescription("Checks whether the submitted verification code matches the verification code sent to the specified email address. If valid, the email is marked as verified.")
            .Produces<VerifyEmailCodeResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/status/{email}", CheckVerificationStatusEndpoint)
            .WithName("CheckEmailVerificationStatus")
            .WithSummary("Check email verification status")
            .WithDescription("Checks whether the specified email address has been verified.")
            .Produces<EmailVerificationStatusResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> RequestEmailVerificationEndpoint(IEmailVerificationService service, EmailVerificationRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return Results.BadRequest("Email is required.");

        EmailVerificationInput input = new(request.Email);

        EmailVerificationRequestResult result = await service.RequestEmailVerificationAsync(input, ct);

        return result.IsSuccess
            ? Results.Accepted(value: result)
            : Results.BadRequest(result.Error);
    }
}