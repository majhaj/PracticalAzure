using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace RedisCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisCacheController : ControllerBase
    {
        private readonly DatabaseMock _databaseMock;
        private readonly IDatabase _redisDb;
        private const string COUNTRIES_KEY = "countries";

        public RedisCacheController()
        {
            _databaseMock = new DatabaseMock();
            var connectionString = "sampleproject1mongowe.redis.cache.windows.net:6380,password=PzxE9XL7xy7iG0Bqw0vay3bulhvucFVezAzCaBOd8iU=,ssl=True,abortConnect=False";
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _redisDb = redis.GetDatabase();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if(_redisDb.KeyExists(COUNTRIES_KEY))
            {
               RedisValue countriesValue = await _redisDb.StringGetAsync(COUNTRIES_KEY);
               var countries = JsonSerializer.Deserialize<Dictionary<string, string>>(countriesValue);
                return Ok(countries);
            }
            else
            {
                var countriesFromDb= await _databaseMock.GetAllCountriesAsync();
                var json = JsonSerializer.Serialize(countriesFromDb);
                _redisDb.StringSet(COUNTRIES_KEY, json);
                _redisDb.KeyExpire(COUNTRIES_KEY, new TimeSpan(1,0,0));
                
                return Ok(countriesFromDb);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string code, string name)
        {
            _databaseMock.AddCountry(code, name);
            _redisDb.KeyDelete(COUNTRIES_KEY);

            return NoContent();
        }
    }
}
