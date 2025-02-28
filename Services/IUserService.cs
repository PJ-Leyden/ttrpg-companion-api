using ttrpg_companion_api.Models.Cosmos;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Models.Response;
using ttrpg_companion_api.Models;

namespace ttrpg_companion_api.Services;

public interface IUserService
{
	Task<User?> GetUser(string username);
	Task<Guid> CreateUser(CreateUserRequest request);
	Task<LoginResponse> LoginUser(LoginUserRequest userRequest);
	Task<UserData?> ValidateUserByJwt(string jwtToken);
}