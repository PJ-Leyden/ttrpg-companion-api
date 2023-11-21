using System;
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

namespace ttrpg_companion_api.Functions;
public class GetWarhammerCharacterSheet
{
    private readonly ILogger<GetWarhammerCharacterSheet> _logger;
	private readonly ISessionService _sessionService;

    public GetWarhammerCharacterSheet(ILogger<GetWarhammerCharacterSheet> log, ISessionService sessionService)
    {
        _logger = log;
		_sessionService = sessionService;
    }

    [FunctionName("GetWarhammerCharacterSheet")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var sessionKey = req.Headers["x-session-key"][0];
        var userId = req.Headers["x-user-id"][0];
        if (!await _sessionService.VerifyUserSession(Guid.Parse(userId), Guid.Parse(sessionKey)))
        {
	        return new UnauthorizedResult();
        }

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<GetWarhammerCharacterSheetRequest>(requestBody);

		return new OkObjectResult("");
    }
}

