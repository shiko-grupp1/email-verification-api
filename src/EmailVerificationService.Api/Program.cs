using EmailVerificationService.Api.Endpoints;
using EmailVerificationService.Api.OpenApi;
using EmailVerificationService.Api.Security;
using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Application.Options;
using EmailVerificationService.Application.Services;
using EmailVerificationService.Infrastructure.Caching;
using EmailVerificationService.Infrastructure.Messaging;
using EmailVerificationService.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiConfiguration();
builder.Services.AddCorsConfiguration();

builder.Services.Configure<EmailVerificationOptions>(
    builder.Configuration.GetSection("EmailVerification"));
builder.Services.Configure<AzureServiceBusOptions>(
    builder.Configuration.GetSection("AzureServiceBus"));

builder.Services.AddScoped<IEmailVerificationService, EmailVerificationManager>();
builder.Services.AddScoped<IVerificationEmailPublisher, ServiceBusVerificationEmailPublisher>();
builder.Services.AddScoped<IEmailVerificationCodeStore, EmailVerificationCodeStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApiEndpoints();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapEmailVerificationEndpoints();

app.Run();


