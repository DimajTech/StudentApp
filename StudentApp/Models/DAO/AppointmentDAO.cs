using Microsoft.Data.SqlClient;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;
using StudentApp.Service;

namespace StudentApp.Models.DAO
{
    public class AppointmentDAO
    {
        private readonly IConfiguration _configuration;
        string connectionString;

        public AppointmentDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public int CreateAppointment(AppointmentDTO appointment)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        using (SqlCommand command = new SqlCommand("CreateAppointment", connection, transaction))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Id", appointment.Id);
                            command.Parameters.AddWithValue("@Date", appointment.Date);
                            command.Parameters.AddWithValue("@Mode", appointment.Mode);
                            command.Parameters.AddWithValue("@Status", "pending");
                            command.Parameters.AddWithValue("@CourseId", appointment.CourseId);
                            command.Parameters.AddWithValue("@StudentId", appointment.StudentId);

                            result = command.ExecuteNonQuery();
                        }

                        
                        using (SqlCommand command = new SqlCommand("GetProfessorEmail", connection, transaction))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@CourseId", appointment.CourseId);

                            var professorEmail = command.ExecuteScalar()?.ToString() ?? string.Empty;

                            // Enviar el correo
                            if (!string.IsNullOrEmpty(professorEmail))
                            {
                                SendEmail.AppointmentEmail(professorEmail);
                            }
                        }

                        
                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }


        public Appointment GetAppointment(Guid id)
        {
            Appointment appointment = null;

      
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("GetAppointmentById", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) 
                    {
                        appointment = new Appointment
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
                catch (SqlException) 
                {
                    throw; 
                }
                return appointment;
            }
        }

        //El estudiente puede ver todos las horas consultas solicitadas

        public List<Appointment> GetAll(string email)
        {
            List<Appointment> appointments = new List<Appointment>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand("GetAppointmentsByStudent", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", email);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                            Date = Convert.ToDateTime(reader["Date"]),
                            Mode = reader["Mode"].ToString(),
                            Status = reader["Status"].ToString(),
                            Course = new Course(
                                reader["Code"].ToString(),
                                reader["Name"].ToString(),
                                null, null, 0, true
                            ),
                            ProfessorComment = reader["ProfessorComment"].ToString()
                        });
                    }
                    connection.Close();
                }
                catch (SqlException) 
                {
                    throw; 
                }
                return appointments;
            }
          
        }



        //Endpoint para Professor
        public int UpdateAppointment(UpdateAppointmentDTO updatedAppointment)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateAppointment", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", updatedAppointment.Id);
                        command.Parameters.AddWithValue("@Status", (object)updatedAppointment.Status ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ProfessorComment", (object)updatedAppointment.ProfessorComment ?? DBNull.Value);

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

    }
}
