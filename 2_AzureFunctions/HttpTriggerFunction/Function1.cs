using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HttpTriggerFunction;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }

    [Function("ReadingFromRequest")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route="ReadingFromRequest/{name}")] HttpRequest req, string name)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        _logger.LogInformation(name);
        return new OkObjectResult("Welcome to Azure Functions!");
    }

    [Function("
}