using Microsoft.Data.SqlClient;
using StudentApp.Models.Entity;

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
        public int Insert(Appointment appointment)
        {
            int result = 0; //Saves 1 or 0 depending on the insertion result
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("InsertAppointment", connection);
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
        public Appointment Get(string id)
        {
            Appointment appointment = new Appointment();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAppointmentById", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) //ask if an user has been found with the given id
                {
                    //TODO parameters
                }
                connection.Close();
            }
            return appointment;
        }

        //El estudiente puede ver todos las horas consultas solicitadas
        public List<Appointment> Get()
        {
            List<Appointment> appointments = new List<Appointment>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAllAppointments", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    appointments.Add(new Appointment
                    {
                        //TODO
                    });
                }
                connection.Close();
            }
            return appointments;
        }
    }
}
