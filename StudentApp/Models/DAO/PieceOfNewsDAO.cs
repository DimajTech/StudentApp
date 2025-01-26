using Microsoft.Data.SqlClient;
using StudentApp.Models.Entity;

namespace StudentApp.Models.DAO
{
    public class PieceOfNewsDAO
    {
        private readonly IConfiguration _configuration;
        string connectionString;

        public PieceOfNewsDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public List<PieceOfNews> Get()
        {
            List<PieceOfNews> news = new List<PieceOfNews>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAllNews", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    news.Add(new PieceOfNews
                    {
                        //TODO: agregar los datos
                    });
                }
                connection.Close();
            }
            return news;
        }

    }
}
