using System.Net;
using System.Text.Json;
using HttpTriggerFunction.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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

    [Function("CreatingResponseExplicitly")]
    public HttpResponseData CreatingResponseExplicitly([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        PostModel post = new()
        {
            Name = "New post"
        };

        response.WriteStringAsync(JsonSerializer.Serialize(post));
        return response;
    }

    [Function("ReadingFromRequest")]
    public IActionResult ReadingFromRequest([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route="ReadingFromRequest/{name}")] HttpRequest req, string name)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        _logger.LogInformation(name);
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
