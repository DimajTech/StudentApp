using Microsoft.Data.SqlClient;
using StudentApp.Models.Entity;

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


                        command.Parameters.AddWithValue("@Id", advisement.Id);
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


        public Advisement GetById(Guid id)
        {
            Advisement advisement = new Advisement();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAdvisementById", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                {
                    advisement = 
                    {
                        Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                        Date = reader["Date"] != DBNull.Value ? Convert.ToDateTime(reader["Date"]) : DateTime.MinValue,
                        Mode = reader["Mode"].ToString(),
                        Status = reader["Status"].ToString(),
                        Course = new Course(
                               reader["Code"].ToString(),
                               reader["Name"].ToString(),
                               null, null, 0, true // 0 porque no deja pasar null en year, se debe sobrecargar constructor.
                           )
                    };

                }
                connection.Close();
            }
            return advisement;
        }


        public Advisement GetByUser(string email)
        {
            Advisement advisement = new Advisement();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

            }
            return advisement;
        }





        public List<Appointment> GetPublicAdvisements()
        {
            List<Appointment> appointments = new List<Appointment>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                return appointments;
            }

        }



    }
}