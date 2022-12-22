using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using sqlapp.Models;
using System.Data.SqlClient;

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

        public List<Product> GetProducts()
        {
            SqlConnection sqlConnection = GetConnection();

            List<Product> _listProducts = new List<Product>();

            string statement = "Select ProductID, ProductName, Quantity from Products";

            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand(statement, sqlConnection);

            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    Product product = new Product();
                    product.ProductID = rd.GetInt32(0);
                    product.ProductName = rd.GetString(1);
                    product.Quantity = rd.GetInt32(2);
                    _listProducts.Add(product);
                }
            }
            sqlConnection.Close();
            return _listProducts;
        }
    }
}
