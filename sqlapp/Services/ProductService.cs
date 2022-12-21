using sqlapp.Models;
using System.Data.SqlClient;

namespace sqlapp.Services
{
    public class ProductService
    {

        private static string db_source = "appdb204.database.windows.net";

        private static string db_user = "sqladmin";

        private static string db_password = "Password@123";

        private static string db_database = "appdb";

        private SqlConnection GetConnection()
        {
            var _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = db_source;
            _builder.UserID = db_user;
            _builder.Password = db_password;
            _builder.InitialCatalog = db_database;
            return new SqlConnection(_builder.ConnectionString);

        }

        public List<Product> GetProducts()
        {
            SqlConnection sqlConnection = GetConnection();

            List<Product> _listProducts = new List<Product>();

            string statement = "Select ProductID, ProductName, Quantity from Products";

            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand(statement,sqlConnection);

            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                while(rd.Read())
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
