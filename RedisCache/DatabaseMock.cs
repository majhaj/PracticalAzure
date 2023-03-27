namespace RedisCache
{
    public class DatabaseMock
    {
        private static Dictionary<string, string> _countries = new Dictionary<string, string>()
        {
            {"pl", "Poland" },
            {"es", "Spain"},
            {"it", "Italy" },
            {"nl", "Netherlands" },
            {"us", "United states" },
            {"en", "England" }
        };

        public async Task<Dictionary<string, string>> GetAllCountriesAsync()
        {
            await Task.Delay(1000);
            return _countries;
        }

        public void AddCountry(string code, string name)
        {
            _countries.Add(code, name);
        }
    }
}
