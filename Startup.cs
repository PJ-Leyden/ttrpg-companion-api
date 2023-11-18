using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ttrpg_companion_api.Repository;

[assembly: FunctionsStartup(typeof(ttrpg_companion_api.Startup))]

namespace ttrpg_companion_api;

public class Startup : FunctionsStartup
{
	public override void Configure(IFunctionsHostBuilder builder)
	{
		builder.Services.AddScoped<ICosmosDbDataAccess>();
	}
}