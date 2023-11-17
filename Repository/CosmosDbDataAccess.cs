using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttrpg_companion_api.Repository
{
	public static class CosmosDbDataAccess
	{
		private static readonly CosmosClient _client;

		static CosmosDbDataAccess()
		{
			_client = new CosmosClient("AccountEndpoint=https://hs-ttrpg-companion-data.documents.azure.com:443/;AccountKey=wliMac6WrZymZrXtBDLneNCYPGYZiawHEuvx3fsSDhxHKaoXO5dS850PPLWfM82kf41xq5tA3aVeACDbYnP0Iw==;");
		}

		public static async Task<Container> GetContainer()
		{
			Database database = _client.GetDatabase("ttrpg-companion-data");

			var props = new ContainerProperties()
			{
				Id = "warhammer-character-sheets",
				PartitionKeyPath = "/username",
			};

			return await database.CreateContainerIfNotExistsAsync(containerProperties: props);
		}
	}
}
