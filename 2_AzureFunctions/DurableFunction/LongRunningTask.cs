using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DurableFunction;
public static class Function3
{
    // You will have to check output property returned from StatusQueryGetUri
    // to see which of the two tasks finished first.
    [Function(nameof(Function3))]
    public static async Task<string> RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(Function3));
        var expiryTime = TimeSpan.FromSeconds(70);
        var expiryTask = context.CreateTimer(context.CurrentUtcDateTime.Add(expiryTime), CancellationToken.None);

        var resultTask = context.CallActivityAsync<string>(nameof(LongRunningTask), "triggerString");
        await Task.WhenAny(expiryTask, resultTask);

        if (resultTask.IsCompletedSuccessfully)
        {
            return "The long running task had completed before the timer task expired.";
        }

        return "The timer task expired before the long running task completes.";
    }

    [Function(nameof(LongRunningTask))]
    public static async Task<string> LongRunningTask([ActivityTrigger] string name, FunctionContext executionContext)
    {
        await Task.Delay(TimeSpan.FromMinutes(1));
        return "LongRunnintTask is done.";
    }

    [Function($"{nameof(Function3)}_HttpStart")]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger($"{nameof(Function3)}_HttpStart");

        // Function input can be added to request context by providing the second parameter to ScheduleNewOrchestrationInstanceAsync.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(Function3));

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        // Returns an HTTP 202 response with an instance management payload.
        // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}

