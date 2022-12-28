using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using sqlapp.Models;
using StackExchange.Redis;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace sqlapp.Services
{
    public class ProductService : IProductService
    {

        private readonly IConfiguration _configuration;

        private readonly IFeatureManager _featureManager;

        private readonly IConnectionMultiplexer _redis;

        public ProductService(IConfiguration configuration, IFeatureManager featureManager, IConnectionMultiplexer redis)
        {
            _configuration = configuration;
            _featureManager = featureManager;
            _redis = redis; 
        }

        public async Task<bool> IsBeta()
        {
            if(await _featureManager.IsEnabledAsync("isZennie"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration["ConnectionString"]);
        }

        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new List<Product>();
            string key = "productlist";
            IDatabase database = _redis.GetDatabase();
            if (await database.KeyExistsAsync(key))
            {
                long listLength = database.ListLength(key);
                for (int i = 0; i < listLength; i++)
                {
                    string value = database.ListGetByIndex(key, i);
                    Product product = JsonConvert.DeserializeObject<Product>(value);
                    products.Add(product);
                }
            }
            else
            {
                string functionUrl = "https://function204.azurewebsites.net/api/GetProducts?code=BJaFlwBGnKkERqbAytfV1rTwBwZ0Mh1S0wT2L7wIP0piAzFu1r1A4A==";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage httpResponse = await client.GetAsync(functionUrl);

                    string content = await httpResponse.Content.ReadAsStringAsync();

                    products = JsonConvert.DeserializeObject<List<Product>>(content);

                    foreach(var product in products)
                    {
                        database.ListRightPush(key, JsonConvert.SerializeObject(product));
                    }

                }
            }
            return products;
        }
    }
}
