using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ttrpg_companion_api.Repository;
using ttrpg_companion_api.Services;

[assembly: FunctionsStartup(typeof(ttrpg_companion_api.Startup))]

namespace ttrpg_companion_api;

public class Startup : FunctionsStartup
{
	public override void Configure(IFunctionsHostBuilder builder)
	{
		builder.Services.AddScoped<ICosmosDbDataAccess, CosmosDbDataAccess>();
		builder.Services.AddScoped<ICreateUserService, CreateUserService>();
		builder.Services.AddLogging((config) =>
		{
			config.AddApplicationInsights(
				configureTelemetryConfiguration: (config) => 
					config.ConnectionString = "InstrumentationKey=ceb89827-af31-4492-a54d-eb3c4b6bc446;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/",
				configureApplicationInsightsLoggerOptions: (options) => { });
		});
	}
}