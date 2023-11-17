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
using ttrpg_companion_api.Models;
using ttrpg_companion_api.Repository;

namespace ttrpg_companion_api.Functions;
public class GetWarhammerCharacterSheet
{
    private readonly ILogger<GetWarhammerCharacterSheet> _logger;

    public GetWarhammerCharacterSheet(ILogger<GetWarhammerCharacterSheet> log)
    {
        _logger = log;
    }

    [FunctionName("GetWarhammerCharacterSheet")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

		var test = new CosmosDbContainers();
		
		string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<WarhammerCharacterSheet>(requestBody);

        var container = await CosmosDbDataAccess.GetContainer();

        await container.CreateItemAsync(data);

		string responseMessage = data.Username;

        return new OkObjectResult(responseMessage);
    }
}

