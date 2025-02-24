using Microsoft.Data.SqlClient;
using StudentApp.Models.DTO;
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
                using (SqlCommand command = new SqlCommand("GetAllAvailableCourses", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                Code = reader["Code"].ToString(),
                                Name = reader["Name"].ToString(),
                                ProfessorId = reader["ProfessorId"].ToString(), // hay campos que en la bd pueden ser null ->  aca deberia manejarse algo para esos casos
                                Semester = reader["Semester"].ToString(),
                                Year = Convert.ToInt32(reader["Year"]),
                                IsActive = true
                            });
                        }
                    }
                }
                connection.Close();
            }

            return courses;
        }

        public Course GetByCode(string code)
        {
            Course course = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetCourseByCode", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Code", code);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            course = new Course
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                Code = reader["Code"].ToString(),
                                Name = reader["Name"].ToString(),
                                ProfessorId = reader["ProfessorId"].ToString(),
                                Semester = reader["Semester"].ToString(),
                                Year = Convert.ToInt32(reader["Year"]),
                                IsActive = true
                            };
                        }
                    }
                }
                connection.Close(); 
            }

            return course;
        }
     
        public int PostCourse(CourseCrudDTO course)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("CreateCourse", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        command.Parameters.AddWithValue("@id", course.id);
                        command.Parameters.AddWithValue("@code", course.code);
                        command.Parameters.AddWithValue("@name", course.name);
                        command.Parameters.AddWithValue("@professorId", course.professorId);
                        command.Parameters.AddWithValue("@semester", course.semester);
                        command.Parameters.AddWithValue("@year", course.year);
                        command.Parameters.AddWithValue("@isActive", course.isActive);

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

        public int Delete(string id)
        {
            int result = 0; //Saves 1 or 0 depending on the insertion result
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("DeleteCourse", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
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

        public int Update(CourseCrudDTO course)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("UpdateCourse", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", course.id);
                        command.Parameters.AddWithValue("@code", course.code);
                        command.Parameters.AddWithValue("@name", course.name);
                        command.Parameters.AddWithValue("@professorId", course.professorId);
                        command.Parameters.AddWithValue("@semester", course.semester);
                        command.Parameters.AddWithValue("@year", course.year);
                        command.Parameters.AddWithValue("@isActive", course.isActive);

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
    }
}
