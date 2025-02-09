using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using StudentApp.Models.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentApp.Models.DAO
{
    public class AdvisementDAO
    {
        private readonly IConfiguration _configuration;
        string connectionString;

        public AdvisementDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public int Create(Advisement advisement)
        {
            int result = 0; //Saves 1 or 0 depending on the insertion result
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("CreateAdvisement", connection); //todo stored procedure
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                        command.Parameters.AddWithValue("@CourseId", advisement.Course.Id);
                        command.Parameters.AddWithValue("@Content", advisement.Content);
                        command.Parameters.AddWithValue("@Status", advisement.Status);
                        command.Parameters.AddWithValue("@IsPublic", advisement.IsPublic);
                        command.Parameters.AddWithValue("@StudentId", advisement.User.Id);
                        command.Parameters.AddWithValue("@CreatedAt", advisement.CreatedAt);

                        result = command.ExecuteNonQuery();
                        connection.Close();
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


        public Advisement GetById(string id)
        {
            Advisement advisement = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAdvisementById", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    advisement = new Advisement
                    {
                        Id = reader["Id"].ToString(),
                        Course = new Course(
                            reader["CourseCode"].ToString(),
                            reader["CourseName"].ToString(),
                            null, null, 0, true
                        ), // constructor sobrecargado 
                        Content = reader["Content"].ToString(),
                        Status = reader["Status"].ToString(),
                        IsPublic = reader["IsPublic"] != DBNull.Value && Convert.ToBoolean(reader["IsPublic"]),
                        User = new User(
                                    reader["UserId"] != DBNull.Value ? reader["UserId"].ToString() : string.Empty,
                                    reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty
                                ),
                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue
                    };
                }
                connection.Close();
            }
            return advisement;
        }

        public IEnumerable<Advisement> GetByUser(string email)
        {
            List<Advisement> advisements = new List<Advisement>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetAdvisementByUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Advisement advisement = new Advisement
                            {
                                Id = reader["Id"].ToString(),
                                Course = new Course(
                                    reader["CourseCode"].ToString(),
                                    reader["CourseName"].ToString(),
                                    null, null, 0, true
                                ),
                                Content = reader["Content"].ToString(),
                                Status = reader["Status"].ToString(),
                                IsPublic = reader["IsPublic"] != DBNull.Value && Convert.ToBoolean(reader["IsPublic"]),
                                User = new User(
                                    reader["UserId"] != DBNull.Value ? reader["UserId"].ToString() : string.Empty,
                                    reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty
                                ),
                                CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue
                            };
                            advisements.Add(advisement);
                        }
                    }
                }
            }

            return advisements;
        }

        public IEnumerable<Advisement> GetPublicAdvisements()
        {
            List<Advisement> advisements = new List<Advisement>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetPublicAdvisements", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Advisement advisement = new Advisement
                            {
                                Id = reader["Id"].ToString(),
                                Course = new Course(
                                    reader["CourseCode"].ToString(),
                                    reader["CourseName"].ToString(),
                                    null, null, 0, true
                                ),
                                Content = reader["Content"].ToString(),
                                Status = reader["Status"].ToString(),
                                IsPublic = reader["IsPublic"] != DBNull.Value && Convert.ToBoolean(reader["IsPublic"]),
                                User = new User(
                                    reader["UserId"] != DBNull.Value ? reader["UserId"].ToString() : string.Empty,
                                    reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty
                                ),
                                CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue
                            };

                            advisements.Add(advisement);
                        }
                    }
                }
            }

            return advisements;
        }

    }
}
