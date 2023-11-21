using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
    public class AuthenticateUser
    {
        private readonly ILogger<AuthenticateUser> _logger;
		private readonly IUserService _userService;

        public AuthenticateUser(ILogger<AuthenticateUser> log, IUserService userService)
        {
            _logger = log;
			_userService = userService;
        }

        [FunctionName("AuthenticateUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AuthenticateUserRequest>(requestBody);

            var sessionKey = await _userService.AuthenticateUser(data);
            if (sessionKey == null)
            {
	            return new UnauthorizedResult();
            }

            return new OkObjectResult(sessionKey);
        }
    }
}

