using System;
using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Cosmos;

public class User
{
	[JsonProperty(PropertyName = "id")]
	public Guid Id { get; set; }
	[JsonProperty(PropertyName = "firstName")]
	public string FirstName { get; set; }
	[JsonProperty(PropertyName = "lastName")]
	public string LastName { get; set; }
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; }
	[JsonProperty(PropertyName = "password")]
	public byte[] Password { get; set; }
	[JsonProperty(PropertyName = "email")]
	public string Email { get; set; }
}