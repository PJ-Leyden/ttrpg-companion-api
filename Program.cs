using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ttrpg_companion_api.Repository;
using ttrpg_companion_api.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

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

builder.Build().Run();
