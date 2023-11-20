using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Requests;

public class CreateUserRequest
{
	[JsonProperty(PropertyName = "firstName")]
	public string FirstName { get; set; }
	[JsonProperty(PropertyName = "lastName")]
	public string LastName { get; set; }
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; }
	[JsonProperty(PropertyName = "password")]
	public string Password { get; set; }
	[JsonProperty(PropertyName = "email")]
	public string Email { get; set; }
}