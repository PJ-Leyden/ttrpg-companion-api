using System;
using System.Threading.Tasks;
using ttrpg_companion_api.Models.Requests;

namespace ttrpg_companion_api.Services;

public interface ICreateUserService
{
	Task<Guid> CreateUser(CreateUserRequest request);
}