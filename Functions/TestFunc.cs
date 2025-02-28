using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ttrpg_companion_api.Functions
{
    public class TestFunc
    {
        private readonly ILogger<TestFunc> _logger;

        public TestFunc(ILogger<TestFunc> logger)
        {
            _logger = logger;
        }

        [Function("TestFunc")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
