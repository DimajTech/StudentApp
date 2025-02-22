using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentApp.Models.DAO;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;

namespace StudentApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string PROFESSOR_API_URL;

        AppointmentDAO appointmentDAO;

        public AppointmentController(ILogger<AppointmentController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            appointmentDAO = new AppointmentDAO(_configuration);
            PROFESSOR_API_URL = _configuration["EnvironmentVariables:PROFESSOR_API_URL"];
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateNewAppointment([FromBody] AppointmentDTO appointment)
        {
            try
            {
                appointment.Id = Guid.NewGuid().ToString();
                appointment.Status = "pending";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PROFESSOR_API_URL);


                    var postTask = client.PostAsJsonAsync("/api/Appointment/CreateAppointmentFromMVC", appointment);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                    }
                    else
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Response", Error = errorMessage });
                    }

                }


                return Ok(appointmentDAO.CreateAppointment(appointment));
            }
            catch (Microsoft.Data.SqlClient.SqlException e)
            {
                return StatusCode(500, new { message = "An error occurred", error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = e.Message });
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAppointmentById([FromBody] Guid id)
        {
            return Ok(appointmentDAO.GetAppointment(id));


        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllAppointmentsByUser([FromQuery] string email)
        {

            return Json(appointmentDAO.GetAll(email));

        }

        //Endpoint Professor
        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateAppointment(UpdateAppointmentDTO updateAppointment)
        {
            return Ok(appointmentDAO.UpdateAppointment(updateAppointment));
        }

    }
}
