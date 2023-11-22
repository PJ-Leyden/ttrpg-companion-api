using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Requests;

public class LoginUserRequest
{
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; }

	[JsonProperty(PropertyName = "password")]
	public string Password { get; set; }
}