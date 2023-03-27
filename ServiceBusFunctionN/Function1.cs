using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBusFunctionN
{
    public record User(string FirstName, string LastName, string Email);

    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [FunctionName("Function1")]
        public void Run([ServiceBusTrigger("user-registered", "subscription1", Connection = "ServiceBusConnection")] string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var user = JsonSerializer.Deserialize<User>(mySbMsg);

        }
    }
}
