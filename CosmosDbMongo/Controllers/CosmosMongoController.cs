using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CosmosDbMongo.Controllers
{
    public record Product(string Id, string Category, string Name, int Quantity, bool Sale);

    [ApiController]
    [Route("[controller]")]
    public class CosmosMongoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            var products = GetCollection();

            products.InsertOne(product);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var products = GetCollection();

            var product = (await products.FindAsync(p => p.Id == id)).First();
        
            return Ok(product);
        }

        [HttpGet("query-linq")]
        public async Task<IActionResult> GetByQueryLinq()
        {
            var products = GetCollection();

            var result = products.AsQueryable()
                .Where(p => p.Category == "sport")
                .OrderByDescending(p => p.Id)
                .ToList();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var products = GetCollection();
            await products.DeleteOneAsync(p => p.Id == id);

            return NoContent();
        }

        private IMongoCollection<Product> GetCollection()
        {
            var connectionString = "mongodb://mysample1cdmongo:k5bCnNh6IibUzClLcwXgC2sUg9sOmxz2Dcj6tApNbJr4LRf3QSBzQc5JnCxs1Dpcy8arVmuITB7mACDblueswQ==@mysample1cdmongo.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@mysample1cdmongo@";

            MongoClient mongoClient = new MongoClient(connectionString);

            IMongoDatabase database = mongoClient.GetDatabase("ProductsDb");

            var products = database.GetCollection<Product>("products");
            
            return products;
        }
    }
}
