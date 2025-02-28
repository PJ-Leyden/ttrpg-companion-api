using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Models;
using ttrpg_companion_api.Models.Cosmos;
using ttrpg_companion_api.Models.Response;
using ttrpg_companion_api.Repository;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace ttrpg_companion_api.Services;

public class UserService : IUserService
{
	private readonly ICosmosDbDataAccess _dbAccess;
	private readonly TokenValidationParameters _tokenValidationParameters;

	public UserService(ICosmosDbDataAccess dbAccess, TokenValidationParameters tokenValidationParameters)
	{
		_dbAccess = dbAccess;
		_tokenValidationParameters = tokenValidationParameters;
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
		// Verify username and email are not already in use.
		// HTTP 409 if so

		var userId = Guid.NewGuid();
		var usersContainer = await _dbAccess.GetContainer(CosmosDbContainers.Users);

		//Encrypt Password
		var hashedPass = Sha256HashString(request.Password);

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

	public async Task<LoginResponse> LoginUser(LoginUserRequest userRequest)
	{
		LoginResponse authenticationResult = new LoginResponse();

		var user = await GetUser(userRequest.Username);
		if (user == null)
		{
			authenticationResult.Success = false;
			authenticationResult.Errors.Add("Username not found.");
			return authenticationResult;
		}

		var givenPassword = Sha256HashString(userRequest.Password);
		var test = Encoding.ASCII.GetString(givenPassword);
		if (!CompareByteArrays(givenPassword, user.Password))
		{
			authenticationResult.Success = false;
			authenticationResult.Errors.Add("Password incorrect.");
			return authenticationResult;
		}

		// authentication successful so generate jwt token  
		var tokenHandler = new JwtSecurityTokenHandler();

		try
		{
			var secret = Environment.GetEnvironmentVariable("JwtSettingsSecret");
			var key = Encoding.ASCII.GetBytes(secret);

			ClaimsIdentity Subject = new ClaimsIdentity(new[]
			{
				new Claim("UserId", user.Id.ToString()),
				new Claim("FirstName", user.FirstName),
				new Claim("LastName",user.LastName),
				new Claim("Email",user.Email),
				new Claim("Username",user.Username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			});

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = Subject,
				Expires = DateTime.UtcNow.Add(TimeSpan.Parse(Environment.GetEnvironmentVariable("JwtSettingsTokenLifetime"))),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			authenticationResult.Token = tokenHandler.WriteToken(token);
			authenticationResult.Success = true;

			return authenticationResult;
		}
		catch (Exception ex)
		{
			return new LoginResponse()
			{
				Errors = new List<string>(){ex.Message},
				Success = false
			};
		}
	}

	public async Task<UserData?> ValidateUserByJwt(string jwtToken)
	{
		var claims = ValidateAndGetClaimsFromToken(jwtToken);
		if (claims == null)
		{
			return null;
		}

		var userData = new UserData()
		{
			Id = Guid.Parse(claims.FindFirst(c => c.Type == "UserId")?.Value),
			FirstName = claims.FindFirst(c => c.Type == "FirstName")?.Value,
			LastName = claims.FindFirst(c => c.Type == "LastName")?.Value,
			Username = claims.FindFirst(c => c.Type == "Username")?.Value,
			Email = claims.FindFirst(c => c.Type == "Email")?.Value,
		};

		return userData;
	}

	private byte[] Sha256HashString(string s)
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

	private ClaimsPrincipal? ValidateAndGetClaimsFromToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		try
		{
			var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
			if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
			{
				return null;
			}

			return principal;
		}
		catch
		{
			return null;
		}
	}

	private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
	{
		return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
		       jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
			       StringComparison.InvariantCultureIgnoreCase);
	}
}