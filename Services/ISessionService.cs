using System;
using System.Threading.Tasks;

namespace ttrpg_companion_api.Services;

public interface ISessionService
{
	/**
	 * Create a new session for a user and return the session key./
	 */
	Task<Guid> GetOrCreateSessionForUser(Guid userId);
	Task<bool> VerifyUserSession(Guid userId, Guid sessionKey);
}