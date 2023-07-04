using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using TableStorageSample.Models;

namespace TableStorageSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TableStorageController : ControllerBase
    {
        private TableClient _tableClient;

        public TableStorageController()
        {
            var connectionString = "AccountName=account1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

            TableServiceClient tableServiceClient = new TableServiceClient(connectionString);

            _tableClient = tableServiceClient.GetTableClient("employees");
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            await _tableClient.CreateIfNotExistsAsync();

            await _tableClient.AddEntityAsync(employee);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string rowKey, [FromQuery] string partitionKey)
        {
            var employee = await _tableClient.GetEntityAsync<Employee>(partitionKey, rowKey);
            return Ok(employee);
        }

        [HttpGet("query")]
        public async Task<IActionResult> Query()
        {
            var employees = _tableClient.Query<Employee>(e => e.PartitionKey == "IT");
            return Ok(employees);
        }
    }
}
