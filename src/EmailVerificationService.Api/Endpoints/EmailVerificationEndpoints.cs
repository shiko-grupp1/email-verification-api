namespace EmailVerificationService.Api.Endpoints;

public static class EmailVerificationEndpoints
{
    public static void MapEmailVerificationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/verification")
            .WithTags("Verification")
            .WithDescription("Endpoints for verifying emails");
    }
}