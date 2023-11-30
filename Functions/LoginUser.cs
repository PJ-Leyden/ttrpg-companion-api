using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ttrpg_companion_api.Models.Requests;
using ttrpg_companion_api.Services;

namespace ttrpg_companion_api.Functions
{
    public class LoginUser
    {
        private readonly ILogger<LoginUser> _logger;
		private readonly IUserService _userService;

        public LoginUser(ILogger<LoginUser> log, IUserService userService)
        {
            _logger = log;
			_userService = userService;
        }

        [FunctionName("LoginUser")]
        [OpenApiOperation(operationId: "LoginUser", tags: new[] { "TTRPG Companion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
		[OpenApiRequestBody("application/json", typeof(LoginUserRequest))]
		public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
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

