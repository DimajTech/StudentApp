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

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            //Validar las credenciales
            if (AuthenticateUser(email, password))
            {
                //cookie de autenticación
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("AuthCookie", "true", cookieOptions);

                //Redirigir a la SPA
                return Redirect("/");
            }

            return View();
        }

        private bool AuthenticateUser(string email, string password)
        {
            //TODO Lógica de autentificación en la BD. 
            return email == "test@example.com" && password == "password123";
        }
    }

}
