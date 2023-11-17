using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ttrpg_companion_api.Models;

public class CosmosDbContainers
{
	public static string Users = "users";
	public static string WarhammerCharacterSheets = "warhammer-character-sheets";

	private ILogger<CosmosDbContainers> Logger;
	public CosmosDbContainers()
	{
		ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddApplicationInsights(
			configureTelemetryConfiguration: (config) => config.ConnectionString = "InstrumentationKey=ceb89827-af31-4492-a54d-eb3c4b6bc446;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/",
			configureApplicationInsightsLoggerOptions: (options) => {}));
		ILogger logger = factory.CreateLogger("Program");
		logger.LogInformation("Hello World! Logging is {Description}.", "fun");
	}

	public static Dictionary<string, string> PartitionKeys = new()
	{
		{ Users, "/tenant" },
		{ WarhammerCharacterSheets, "/username" }
	};

	public static string GetContainerPartitionKey(string containerName)
	{
		string key = string.Empty;
		if (PartitionKeys.TryGetValue(containerName, out key))
		{
			return key;
		}

		return "";
	}
}