using EmailVerificationService.Api.Endpoints;
using EmailVerificationService.Api.OpenApi;
using EmailVerificationService.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiConfiguration();
builder.Services.AddCorsConfiguration();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApiEndpoints();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapEmailVerificationEndpoints();

app.Run();


