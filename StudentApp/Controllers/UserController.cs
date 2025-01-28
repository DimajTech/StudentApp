using Microsoft.AspNetCore.Mvc;

namespace StudentApp.Controllers
{
    public class UserController : Controller
    {
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
            if (AuthenticateUser(email, password))
            {
                //Configurar la cookie de autenticación
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("AuthCookie", "true", cookieOptions);

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Credenciales inválidas. Intente de nuevo." });
        }

        private bool AuthenticateUser(string email, string password)
        {
            //TODO Lógica de autentificación en la BD. 
            //Se debe validar si el usuario está aprobado por el admin
            return email == "test@example.com" && password == "password123";
        }


        [HttpPost]
        public IActionResult Register(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return Json(new { success = false, message = "Las contraseñas no coinciden." });
            }

            // Lógica para registrar al usuario en la base de datos
            return Json(new { success = true, message = "Usuario registrado exitosamente." });
        }

    }

}
