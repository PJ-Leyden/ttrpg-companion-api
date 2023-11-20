using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ttrpg_companion_api.Models;
using ttrpg_companion_api.Models.Cosmos;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Repository;

namespace ttrpg_companion_api.Services;

public class CreateUserService : ICreateUserService
{
	private readonly ICosmosDbDataAccess _dbAccess;

	public CreateUserService(ICosmosDbDataAccess dbAccess)
	{
		_dbAccess = dbAccess;
	}

	public async Task<Guid> CreateUser(CreateUserRequest request)
	{
		var userId = Guid.NewGuid();
		var usersContainer = await _dbAccess.GetContainer(CosmosDbContainers.Users);

		Encoding ascii = Encoding.ASCII;

		var uniBytes = ascii.GetBytes(request.Password);
		var hashedPass = SHA256.HashData(uniBytes);

		var newUser = new User()
		{
			Id = userId,
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email,
			Username = request.Username,
			Password = hashedPass
		};

		await usersContainer.CreateItemAsync(newUser);
		return userId;
	}
}