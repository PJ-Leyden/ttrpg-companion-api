using System.Threading.Tasks;
using System;
using ttrpg_companion_api.Models.Cosmos;
using ttrpg_companion_api.Models.Requests;

namespace ttrpg_companion_api.Services;

public interface IUserService
{
	Task<User?> GetUser(string username);
	Task<Guid> CreateUser(CreateUserRequest request);
	Task<Guid?> AuthenticateUser(AuthenticateUserRequest request);
}