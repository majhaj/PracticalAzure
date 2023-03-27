using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CosmosDBCore.Controllers
{
    public record Employee(string id, string department, string name, string address);

    [ApiController]
    [Route("[controller]")]
    public class CosmosCoreController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            var container = await GetContainer();
            await container.CreateItemAsync(employee);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string id, [FromQuery] string partitionKey)
        {
            var container = await GetContainer();
            var employee = await container.ReadItemAsync<Employee>(id, new PartitionKey(partitionKey));

            return Ok(employee);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Employee employee)
        {
            var container = await GetContainer();
            await container.UpsertItemAsync(employee);

            return Ok(employee);
        }

        [HttpGet("query")]
        public async Task<IActionResult> Query()
        {
            var sqlQuery = "SELECT * FROM Employees e WHERE e.department = 'IT'";

            var employees = GetEmployees(sqlQuery);

            return Ok(employees);
        }

        [HttpGet("query-linq")]
        public async Task<IActionResult> QueryLinq()
        {
            var container = await GetContainer();
            var employees = container.GetItemLinqQueryable<Employee>(true)
                .Where(e => e.department == "IT")
                .ToList();

            return Ok(employees);
        }

        private async IAsyncEnumerable<Employee> GetEmployees(string sqlQuery)
        {
            var container = await GetContainer();
            var employeesFeed = container.GetItemQueryIterator<Employee>(sqlQuery);

            while (employeesFeed.HasMoreResults)
            {
                var response = await employeesFeed.ReadNextAsync();
                foreach(var employee in response)
                {
                    yield return employee;
                }
            }
        }

        private async Task<Container> GetContainer()
        {
            var connectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            CosmosClient cosmosClient = new CosmosClient(connectionString);

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("Employees");

            return await database.CreateContainerIfNotExistsAsync("Employees", "/department", 400);
        }
    }
}
