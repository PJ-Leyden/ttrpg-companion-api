using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Services;

namespace ttrpg_companion_api.Functions
{
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;
		private readonly IUserService _userService;

        public CreateUser(ILogger<CreateUser> logger, IUserService userService)
        {
            _logger = logger;
			_userService = userService;
        }

		[Function("CreateUser")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
		{
			_logger.LogInformation("C# HTTP trigger function processed a request.");

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<CreateUserRequest>(requestBody);

			var id = await _userService.CreateUser(data);

			return new OkObjectResult($"Successfully Created User with Id: {id}");
		}
	}
}
