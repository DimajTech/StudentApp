using Microsoft.AspNetCore.Mvc;

namespace StudentApp.Controllers
{
    public class AdvisementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
