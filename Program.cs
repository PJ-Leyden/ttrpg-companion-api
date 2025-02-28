using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;
using ttrpg_companion_api.Repository;
using ttrpg_companion_api.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var secret = Environment.GetEnvironmentVariable("JwtSettingsSecret");
if (string.IsNullOrEmpty(secret))
{
	throw new Exception("JwtSettingsSecret must be set for security reasons.");
}

var tokenValidationParameters = new TokenValidationParameters()
{
	ValidateIssuerSigningKey = true,
	IssuerSigningKey =
		new SymmetricSecurityKey(
			Encoding.ASCII.GetBytes(secret)),
	ValidateIssuer = false,
	ValidateAudience = false,
	RequireExpirationTime = false,
	ValidateLifetime = true
};

builder.Services.AddScoped<ICosmosDbDataAccess, CosmosDbDataAccess>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton(typeof(TokenValidationParameters), tokenValidationParameters);

builder.Services.AddLogging((config) =>
{
	config.AddApplicationInsights(
		configureTelemetryConfiguration: (config) =>
			config.ConnectionString = "InstrumentationKey=ceb89827-af31-4492-a54d-eb3c4b6bc446;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/",
		configureApplicationInsightsLoggerOptions: (options) => { });
});

builder.Build().Run();
