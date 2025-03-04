using Microsoft.Azure.Cosmos;
using ttrpg_companion_api.Models;

namespace ttrpg_companion_api.Repository
{
	public class CosmosDbDataAccess : ICosmosDbDataAccess
	{
		private readonly CosmosClient _client;
		
		public CosmosDbDataAccess()
		{
			var connectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
			_client = new CosmosClient(connectionString);
		}

		public async Task<Container> GetContainer(string containerName)
		{
			Database database = _client.GetDatabase("ttrpg-companion-data");

			var props = new ContainerProperties()
			{
				Id = containerName,
				PartitionKeyPath = CosmosDbContainers.GetContainerPartitionKey(containerName),
			};

			return await database.CreateContainerIfNotExistsAsync(containerProperties: props);
		}
	}
}
