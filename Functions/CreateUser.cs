using System.IO;
using System.Net;
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
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;
        private readonly ICreateUserService _createUserService;

        public CreateUser(ILogger<CreateUser> log, ICreateUserService createUserService)
        {
            _logger = log;
			_createUserService = createUserService;
        }

        [FunctionName("CreateUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateUserRequest>(requestBody);

            var id = await _createUserService.CreateUser(data);

            return new OkObjectResult($"Successfully Created User with Id: {id}");
        }
    }
}

