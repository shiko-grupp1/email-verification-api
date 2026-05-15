using Azure.Messaging.ServiceBus;
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
builder.Services.AddMemoryCache();

builder.Services.AddSingleton(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnection")
        ?? throw new InvalidOperationException("AzureServiceBus connecting string is missing.");

    return new ServiceBusClient(connectionString);
});

builder.Services.Configure<EmailVerificationOptions>(
    builder.Configuration.GetSection("EmailVerification"));
builder.Services.Configure<AzureServiceBusOptions>(
    builder.Configuration.GetSection("AzureServiceBus"));

builder.Services.AddScoped<IEmailVerificationService, EmailVerificationManager>();
builder.Services.AddScoped<IVerificationEmailPublisher, ServiceBusVerificationEmailPublisher>();
// en instans som delas över hela applikationen, alltså en lagringsplats
builder.Services.AddSingleton<IEmailVerificationCodeStore, EmailVerificationCodeStore>();

var app = builder.Build();

app.MapOpenApiEndpoints();

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapEmailVerificationEndpoints();

app.Run();


