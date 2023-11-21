using System.Collections.Generic;

namespace ttrpg_companion_api.Models;

public class CosmosDbContainers
{
	public static string Users = "users";
	public static string WarhammerCharacterSheets = "warhammer-character-sheets";
	public static string Sessions = "sessions";

	public static Dictionary<string, string> PartitionKeys = new()
	{
		{ Users, "/username" },
		{ WarhammerCharacterSheets, "/username" },
		{ Sessions, "/userId" }
	};

	public static string? GetContainerPartitionKey(string containerName)
	{
		PartitionKeys.TryGetValue(containerName, out var key);
		return key;
	}
}