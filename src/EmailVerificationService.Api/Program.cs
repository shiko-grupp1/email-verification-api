using EmailVerificationService.Api.Endpoints;
using EmailVerificationService.Api.OpenApi;
using EmailVerificationService.Api.Security;
using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Application.Options;
using EmailVerificationService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiConfiguration();
builder.Services.AddCorsConfiguration();

builder.Services.Configure<EmailVerificationOptions>(
    builder.Configuration.GetSection("EmailVerification"));

builder.Services.AddScoped<IEmailVerificationService, EmailVerificationManager>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApiEndpoints();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapEmailVerificationEndpoints();

app.Run();


