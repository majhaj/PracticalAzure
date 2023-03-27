using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ServiceBusProducent.Controllers
{

    public record User(string FirstName, string LastName, string Email);

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var connectionString = "";

            var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender("user-registered");

            var message = new ServiceBusMessage(JsonSerializer.Serialize(user));

            await sender.SendMessageAsync(message);

            return Ok();
        }
    }
}
