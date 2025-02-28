using System;
using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Cosmos;

public class WarhammerCharacterSheet
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; } = string.Empty;
}