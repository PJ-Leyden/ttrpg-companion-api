using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Cosmos;

public class Session
{
	[JsonProperty(PropertyName = "id")]
	public Guid Id { get; set; }

	[JsonProperty(PropertyName = "userId")]
	public Guid UserId { get; set; }

	[JsonProperty(PropertyName = "sessionKey")]
	public Guid SessionKey { get; set; }

	[JsonProperty(PropertyName = "expiration")]
	public DateTimeOffset Expiration { get; set; }
}