using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Cryptography;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Models;
using ttrpg_companion_api.Models.Cosmos;
using ttrpg_companion_api.Repository;

namespace ttrpg_companion_api.Services;

public class UserService : IUserService
{
	private readonly ICosmosDbDataAccess _dbAccess;
	private ISessionService _sessionService;

	public UserService(ICosmosDbDataAccess dbAccess, ISessionService sessionService)
	{
		_dbAccess = dbAccess;
		_sessionService = sessionService;
	}

	public async Task<User?> GetUser(string username)
	{
		var container = await _dbAccess.GetContainer(CosmosDbContainers.Users);
		var user = container.GetItemLinqQueryable<User>(true).Where(u => u.Username == username).AsEnumerable().FirstOrDefault();
		if (user == null)
		{
			throw new InvalidOperationException($"No user with the username {username}.");
		}

		return user;
	}

	public async Task<Guid> CreateUser(CreateUserRequest request)
	{
		var userId = Guid.NewGuid();
		var usersContainer = await _dbAccess.GetContainer(CosmosDbContainers.Users);

		var hashedPass = HashString(request.Password);

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

	public async Task<Guid?> AuthenticateUser(AuthenticateUserRequest request)
	{
		var user = await GetUser(request.Username);
		if (user == null)
		{
			return null; 
		}

		var givenPassword = HashString(request.Password);
		if (CompareByteArrays(givenPassword, user.Password))
		{
			return await _sessionService.GetOrCreateSessionForUser(user.Id);
		}

		return null;
	}

	private byte[] HashString(string s)
	{
		Encoding ascii = Encoding.ASCII;

		var uniBytes = ascii.GetBytes(s);
		return SHA256.HashData(uniBytes);
	}

	private bool CompareByteArrays(byte[] a, byte[] b)
	{
		for (int i = 0; i < a.Length - 1; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}

		return true;
	}
}