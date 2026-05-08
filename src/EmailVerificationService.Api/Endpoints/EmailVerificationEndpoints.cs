namespace EmailVerificationService.Api.Endpoints;

public static class EmailVerificationEndpoints
{
    public static void MapEmailVerificationEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/verification")
            .WithTags("Email Verification")
            .WithDescription("Endpoints for verifying emails");

    }
}