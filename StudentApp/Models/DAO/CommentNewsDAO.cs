using Microsoft.Data.SqlClient;
using StudentApp.Models.Entity;

namespace StudentApp.Models.DAO
{
    public class CommentNewsDAO
    {
        private readonly IConfiguration _configuration;
        string connectionString;

        public CommentNewsDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public int Insert(CommentNews commentNews)
        {
            int result = 0; //Saves 1 or 0 depending on the insertion result
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("InsertComment", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        //TODO AddValue

                        result = command.ExecuteNonQuery();
                        connection.Close();

                    }
                }
                catch (SqlException e)
                {
                    throw;
                }

            return result;

        }

        //Possible Method: Bring all the comments by PieceOfNewsId
    }
}
