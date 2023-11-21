using System;
using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Cosmos;

public class User
{
	[JsonProperty(PropertyName = "id")]
	public Guid Id { get; set; }
	
	[JsonProperty(PropertyName = "firstName")]
	public string FirstName { get; set; } = string.Empty;
	
	[JsonProperty(PropertyName = "lastName")]
	public string LastName { get; set; } = string.Empty;
	
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; } = string.Empty;

	[JsonProperty(PropertyName = "password")]
	public byte[] Password { get; set; } = Array.Empty<byte>();

	[JsonProperty(PropertyName = "email")]
	public string Email { get; set; } = string.Empty;
}