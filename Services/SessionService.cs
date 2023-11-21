using System;
using System.Linq;
using System.Threading.Tasks;
using ttrpg_companion_api.Models;
using ttrpg_companion_api.Models.Cosmos;
using ttrpg_companion_api.Repository;

namespace ttrpg_companion_api.Services;

public class SessionService : ISessionService
{
	private readonly ICosmosDbDataAccess _dbAccess;

	public SessionService(ICosmosDbDataAccess dbAccess)
	{
		_dbAccess = dbAccess;
	}

	public async Task<Guid> GetOrCreateSessionForUser(Guid userId)
	{
		var container = await _dbAccess.GetContainer(CosmosDbContainers.Sessions);
		
		// If an existing non-expired session exists, use that instead.
		var sessions = container.GetItemLinqQueryable<Session>(true).Where(s => s.UserId == userId && s.Expiration > DateTimeOffset.Now)
			.AsEnumerable().ToList();

		if (sessions.Any())
		{
			return sessions.First().SessionKey;
		}

		// If not, create a new session.
		var session = new Session()
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			SessionKey = Guid.NewGuid(),
			Expiration = DateTimeOffset.Now + TimeSpan.FromHours(1)
		};

		await container.CreateItemAsync(session);

		return session.SessionKey;
	}

	public async Task<bool> VerifyUserSession(Guid userId, Guid sessionKey)
	{
		var container = await _dbAccess.GetContainer(CosmosDbContainers.Sessions);
		var sessions = container.GetItemLinqQueryable<Session>(true).Where(s => s.UserId == userId && s.Expiration > DateTimeOffset.Now)
			.AsEnumerable().ToList();

		if (sessions.Any(s => s.SessionKey == sessionKey))
		{
			return true;
		}

		return false;
	}
}