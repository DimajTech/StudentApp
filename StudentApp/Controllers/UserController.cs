using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentApp.Models.DAO;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;
using StudentApp.Service;
using StudentApp.Service.AdminApp;
using StudentApp.Service.ProfessorApp;

namespace StudentApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        UserDAO userDAO;
        private readonly string PROFESSOR_API_URL;

        public UserController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            userDAO = new UserDAO(_configuration);
            PROFESSOR_API_URL = _configuration["EnvironmentVariables:PROFESSOR_API_URL"];

        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult Register()
        {
            return View(); //ASP.NET Core busca automáticamente Views/Shared/Register.cshtml
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                User user = userDAO.GetByEmail(login.Email);
                bool success = false;
                string message = "";
                string userId = "";
                string role = "";
                string picture = "";

                if (user != null && user.Password == login.Password)
                {
                    if (user.RegistrationStatus == "accepted")
                    {
                        if (user.IsActive != null && user.IsActive == true)
                        {
                            var authData = $"{user.Email} {user.Id}";

                            var cookieOptions = new CookieOptions
                            {
                                //HttpOnly = false,
                                Secure = true,
                                Expires = DateTime.UtcNow.AddHours(3)
                            };

                            Response.Cookies.Append("AuthCookie", authData, cookieOptions);

                            // Devolver los datos en la respuesta JSON
                            success = true;
                            userId = user.Id.ToString();
                            role = user.Role.ToString();
                            picture = user.Picture.ToString();
                        }
                        else
                        {
                            message = "Su usuario se encuentra inactivo. Contacte un administrador.";
                        }
                    }
                    else
                    {
                        message = "Su usuario aún no ha sido aprobado.";
                    }
                }
                else
                {
                    message = "Credenciales inválidas.";
                }

                return Json(new { success, message, userId, role, picture });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor." });
            }
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody] InsertStudentDTO user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.UtcNow;


            bool success = false;
            string message = "";
            try
            {
                if (userDAO.GetByEmail(user.Email).Email == null)
                {
                    int result = userDAO.Insert(user);

                    if (result == 1)
                    {

                        SendEmail.RegisterEmail(user);
                        var professorService = new ProfessorUserService(_configuration);
                        professorService.RegisterStudentToProfessor(user);
                        var adminService = new AdminUserService(_configuration);
                        adminService.RegisterStudentToAdmin(user);
                        success = true;
                        message = "Su solicitud ha sido enviada correctamente. Revisa tu correo. ";
                    }
                    else
                    {
                        success = false;
                        message = "No ha sido posible hacer la solicitud de registro. Intente de nuevo más tarde. ";
                    }
                }
                else
                {
                    success = false;
                    message = "El correo electrónico ya ha sido registrado en el sistema. ";
                }

            }
            catch (SqlException e)
            {
                success = false;
                message = "Ha ocurrido un error inesperado en el sistema. Intente de nuevo más tarde.";
            }
            return Json(new { success = success, message = message });

        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetByEmail([FromQuery] string email)
        {
            try
            {
                User user = userDAO.GetByEmail(email);

                if (user != null)
                {
                    return Ok(user);
                }

                return BadRequest();
            }
            catch (SqlException e)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the user.", error = e.Message });
            }

        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateUser([FromBody] UpdateStudentDTO user)
        {
            try
            {
                //Todo: Revisar Profesor
                var service = new ProfessorUserService(_configuration);
                service.UpdateStudent(user);

                return Ok(userDAO.Update(user));
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                //Todo: conectar con las otras API
                var result = userDAO.Delete(id);
                return Ok(new { success = true, result = result });
            }
            catch (SqlException e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }

        //----------------------- PROFESSOR-TO-STUDENT METHODS -----------------------
        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateProfessor([FromBody] UpdateStudentDTO newValues)
        {
            try
            {
                return Ok(userDAO.Update(newValues));
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
        }

        //--------------------------ADMIN-TO-STUDENT METHODS---------------------------

        [HttpPost]
        [Route("[action]")]
        public IActionResult RegisterProfessor([FromBody] InsertProfessorDTO user)
        {
            return Ok(userDAO.InsertProfessor(user));
            }

        [HttpPatch]
        [Route("[action]")]
        public IActionResult ChangeUserStatus(string id, [FromBody] StatusUpdateDTO status)
        {
            try
            {
                return Ok(userDAO.UpdateStatus(id, status));
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
