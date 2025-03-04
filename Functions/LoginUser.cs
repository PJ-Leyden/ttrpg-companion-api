using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Services;

namespace ttrpg_companion_api.Functions
{
    public class LoginUser
    {
        private readonly ILogger<LoginUser> _logger;
        private readonly IUserService _userService;

        public LoginUser(ILogger<LoginUser> logger, IUserService userService)
        {
	        _logger = logger;
	        _userService = userService;
        }

        [Function("LoginUser")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
			_logger.LogInformation("C# HTTP trigger function processed a request.");
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<LoginUserRequest>(requestBody);

			if (data == null)
			{
				return new BadRequestResult();
			}

			try
			{
				var response = await _userService.LoginUser(data);
				if (!response.Success)
				{
					return new UnauthorizedObjectResult(response.Errors);
				}

				return new OkObjectResult(response.Token);
			}
			catch (Exception ex)
			{
				return new UnauthorizedObjectResult(ex.Message);

			}
		}
    }
}
