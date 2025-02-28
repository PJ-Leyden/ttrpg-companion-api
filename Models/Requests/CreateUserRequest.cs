using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Requests;

public class CreateUserRequest
{
	[JsonProperty(PropertyName = "firstName")]
	public string FirstName { get; set; } = string.Empty;

	[JsonProperty(PropertyName = "lastName")]
	public string LastName { get; set; } = string.Empty;

	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; } = string.Empty;

	[JsonProperty(PropertyName = "password")]
	public string Password { get; set; } = string.Empty;

	[JsonProperty(PropertyName = "email")]
	public string Email { get; set; } = string.Empty;
}