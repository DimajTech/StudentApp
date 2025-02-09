using Microsoft.AspNetCore.Mvc;
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

        public int Insert(PieceOfNews news)
        {
            int result = 0; //Saves 1 or 0 depending on the insertion result

            if (news.User.Role =="President")
			{
                using (SqlConnection connection = new SqlConnection(connectionString))
                    try
                    {
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand("InsertNews", connection);
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Title", news.Title);
                            //command.Parameters.AddWithValue("@File", null);
                            command.Parameters.AddWithValue("@Picture", news.Picture);
                            command.Parameters.AddWithValue("@AuthorID", news.User.Id);
                            command.Parameters.AddWithValue("@Description", news.Description);



                            result = command.ExecuteNonQuery();
                            connection.Close();

                        }
                    }
                    catch (SqlException e)
                    {
                        throw;
                    }
            }
            

            return result;

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
						Id = reader["Id"].ToString(),
						Title = reader["Title"].ToString(),
						Description = reader["Description"].ToString(),
						Picture = reader["Picture"].ToString(),
						Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
						User = new User(reader["AuthorID"].ToString(), reader["AuthorName"].ToString(), null, null, reader["AuthorRole"].ToString()),
					});
				}
				connection.Close();
			}
			return news;
		}
		public PieceOfNews Get(string id)
		{
			PieceOfNews news = new PieceOfNews();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand("GetNewsById", connection);

				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@Id", id);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read()) //Asks if a user has been found with the given email
				{
					news.Title = reader["Title"].ToString();
					news.Description = reader["Description"].ToString();
					news.Picture = reader["Picture"].ToString();
					news.Date = DateOnly.FromDateTime((DateTime)reader["Date"]);
					news.User = new User(reader["AuthorID"].ToString(), reader["AuthorName"].ToString(), null, null, reader["AuthorRole"].ToString());
				}
				connection.Close();
			}
			return news;
		}

	}
}
