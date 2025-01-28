using Microsoft.Data.SqlClient;
using StudentApp.Models.Entity;

namespace StudentApp.Models.DAO 
{ 
    public class CourseDAO
    {
        private readonly IConfiguration _configuration;
        string connectionString;

        public CourseDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<Course> Get()
        {
            List<Course> courses = new List<Course>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAllAvailableCourses", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    courses.Add(new Course
                    {
                        //TODO: agregar los datos
                    });
                }
                connection.Close();
            }
            return courses;
        }
    }
}
