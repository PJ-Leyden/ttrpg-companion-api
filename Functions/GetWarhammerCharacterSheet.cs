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

namespace ttrpg_companion_api.Functions;
public class GetWarhammerCharacterSheet
{
    private readonly ILogger<GetWarhammerCharacterSheet> _logger;

    public GetWarhammerCharacterSheet(ILogger<GetWarhammerCharacterSheet> log)
    {
        _logger = log;
    }

    [FunctionName("GetWarhammerCharacterSheet")]
    [OpenApiOperation(operationId: "GetWarhammerCharacterSheet", tags: new[] { "TTRPG Companion" })]
	[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
	[OpenApiRequestBody("application/json", typeof(GetWarhammerCharacterSheetRequest))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<GetWarhammerCharacterSheetRequest>(requestBody);

		return new OkObjectResult("");
    }
}

