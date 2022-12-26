using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using sqlapp.Models;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace sqlapp.Services
{
    public class ProductService : IProductService
    {

        private readonly IConfiguration _configuration;

        private readonly IFeatureManager _featureManager;

        public ProductService(IConfiguration configuration, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _featureManager = featureManager;
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
            string functionUrl = "https://function204.azurewebsites.net/api/GetProducts?code=BJaFlwBGnKkERqbAytfV1rTwBwZ0Mh1S0wT2L7wIP0piAzFu1r1A4A==";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage httpResponse = await client.GetAsync(functionUrl);

                string content = await httpResponse.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Product>>(content);
            }
        }
    }
}
