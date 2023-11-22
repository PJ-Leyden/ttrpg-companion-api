using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Requests;

public class GetWarhammerCharacterSheetRequest
{
	[JsonProperty(PropertyName = "id")]
	public int Id { get; set; }
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; } = string.Empty;


}