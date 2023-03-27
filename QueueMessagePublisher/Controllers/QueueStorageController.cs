using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using QueueMessagePublisher.Models;
using System.Text.Json;

namespace QueueMessagePublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueStorageController : ControllerBase
    {
        [HttpPost("publish")]
        public async Task<IActionResult> Publish(ReturnDto returnDto)
        {
            var connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            var queueName = "returns";

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            await queueClient.CreateIfNotExistsAsync();

            var serializedMessage = JsonSerializer.Serialize(returnDto);

            await queueClient.SendMessageAsync(serializedMessage);

            return Ok();        
        }
    }
}
