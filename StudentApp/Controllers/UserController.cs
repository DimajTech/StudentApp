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

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                User user = userDAO.GetByEmail(email);
                bool success = false;
                string message = "";
                string userId = "";
                string role = "";

                if (user != null && user.Password == password)
                {
                    if (user.RegistrationStatus == "accepted")
                    {
                        if (user.IsActive)
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

                return Json(new { success, message, userId, role });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor." });
            }
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
                        //TODO: Agregar funcionalidad de correo
                        using (var client = new HttpClient())
                        {
                            Email email = new Email();
                            email.ToUser = user.Email;
                            email.Subject = "Registro exitoso";
                            email.Content = "<html lang='es'>\n<head>\n    <meta charset='UTF-8'>\n    <meta name='viewport' content='width=device-width, initial-scale=1.0'>\n    <title>Email de Bienvenida</title>\n    <style>\n        body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }\n        .container { max-width: 600px; background: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); text-align: center; }\n        .logo { width: 300px; margin-bottom: 20px; }\n        h1 { color: #00693e; }\n        p { color: #333; font-size: 16px; }\n        .footer { margin-top: 20px; font-size: 12px; color: #666; }\n    </style>\n</head>\n<body>\n    <div class='container'>\n        <img src='https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEh5057rSlx1GVQGuaLOV0oMvYZnSlUUm5szF7HaA_sSFdbVuAQ-oSBKMjBHjjX2YSZtBaIhY6ccW7jGKp3j_mi-eYL58Sz2oS3ZRDMY0V1bzOEaRUsnUsMqEGvaT7-zcwqIxGdkmEi-0DsO/w1200-h630-p-k-no-nu/UCR+logo+transparente.png' alt='Logo UCR' class='logo'>\n        <h1>¡Bienvenido a la Universidad de Costa Rica!</h1>\n        <p>Hola, <strong>" + user.Name + "</strong></p>\n        <p>Gracias por registrarse en nuestro sitio web. Actualmente estamos procesando su solicitud, y su cuenta estará activa una vez que sea validada por el administrador.</p>\n        <p class='footer'>Si tiene alguna duda, no dude en ponerte en contacto con nosotros.</p>\n    </div>\n</body>\n</html>"
;
                            client.BaseAddress = new Uri("http://localhost:5092/api/SendEmail/");

                            var postTask = client.PostAsJsonAsync("SendEmail", email);
                            postTask.Wait();

                        }
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
        public IActionResult UpdateUser([FromBody] User user)
        {
            try
            {
                return Ok(userDAO.Update(user));
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery] string id)
        {
            try
            {
                var result = userDAO.Delete(id);
                return Ok(new { success = true, result = result });
            }
            catch (SqlException e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }
    }
}
