using EmailVerificationService.Api.Requests;
using EmailVerificationService.Api.Security;
using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Application.Inputs;
using EmailVerificationService.Application.Shared.Results;

namespace EmailVerificationService.Api.Endpoints;

public static class EmailVerificationEndpoints
{
    public static void MapEmailVerificationEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/verification")
            .AddEndpointFilter<ApiKeyAuthFilter>()
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
            // gör ingen autentisering. Kollar bara om kod finns/giltig
            .Produces(StatusCodes.Status400BadRequest);
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

    private static async Task<IResult> VerifyEmailCodeEndpoint(IEmailVerificationService service, VerifyEmailCodeRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Code))
            return Results.BadRequest("Email and code are required.");

        VerifyEmailCodeInput input = new(request.Email, request.Code);

        VerifyEmailCodeResult result = await service.VerifyEmailCodeAsync(input, ct);

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result.Error);
    }

}