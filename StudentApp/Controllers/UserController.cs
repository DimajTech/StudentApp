using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentApp.Models.DAO;
using StudentApp.Models.Entity;

namespace StudentApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        UserDAO userDAO;

        public UserController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            userDAO = new UserDAO(_configuration);

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(); //ASP.NET Core busca automáticamente Views/Shared/Register.cshtml
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            //TODO Encerrar esto en un try catch
            User user = userDAO.GetByEmail(email);
            bool success = false;
            string message = "";

            if (user != null && user.Password == password)
            {

                //TODO verificar que esté aceptado y activo
                if (user.RegistrationStatus == "accepted")
                {
                    if (user.IsActive)
                    {
                        //Configurar la cookie de autenticación
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddHours(1)
                        };
                        Response.Cookies.Append("AuthCookie", "true", cookieOptions);
                        success = true;
                    }
                    else
                    {
                        success = false;
                        message = "Su usuario se encuentra inactivo. Por favor contacte un administrador.";
                    }
                }
                else
                {
                    message = "Su usuario aún no ha sido aprobado por un administrador.";
                }
            }
            else
            {
                success = false;
                message = "Credenciales inválidas. Intente de nuevo.";
            }

            return Json(new { success = success, message = message });
        }


        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            bool success = false;
            string message = "";
            try
            {
                if (userDAO.GetByEmail(user.Email).Email == null)
                {
                    int result = userDAO.Insert(user);

                    if(result == 1)
                    {
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


    }

}
