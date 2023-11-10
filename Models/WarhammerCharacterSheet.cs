using System;
using Newtonsoft.Json;

namespace ttrpg_companion_api.Models;

public class WarhammerCharacterSheet
{
	[JsonProperty(PropertyName = "id")]
	public Guid Id { get; set; }
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; } = string.Empty;
}