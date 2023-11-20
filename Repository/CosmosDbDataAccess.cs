using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ttrpg_companion_api.Models;

namespace ttrpg_companion_api.Repository
{
	public class CosmosDbDataAccess : ICosmosDbDataAccess
	{
		private readonly CosmosClient _client;
		
		public CosmosDbDataAccess()
		{
			_client = new CosmosClient("AccountEndpoint=https://hs-ttrpg-companion-data.documents.azure.com:443/;AccountKey=wliMac6WrZymZrXtBDLneNCYPGYZiawHEuvx3fsSDhxHKaoXO5dS850PPLWfM82kf41xq5tA3aVeACDbYnP0Iw==;");
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
