using Microsoft.Azure.Cosmos;

namespace ttrpg_companion_api.Repository;

public interface ICosmosDbDataAccess
{
	Task<Container> GetContainer(string containerName);
}