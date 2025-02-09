using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentApp.Models.DAO;
using StudentApp.Models.Entity;

namespace StudentApp.Controllers
{
    public class AdvisementController : Controller
    {
        private readonly ILogger<AdvisementController> _logger;
        private readonly IConfiguration _configuration;

        AdvisementDAO advisementDAO;

        public AdvisementController(ILogger<AdvisementController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            advisementDAO = new AdvisementDAO(_configuration);
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CreateNewAdvisement([FromBody] Advisement advisement)
        {
            try
            {
                return Ok(advisementDAO.Create(advisement));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }


        }



        public IActionResult GetAdvisementById([FromBody] string id)
        {
            return Ok(advisementDAO.GetById(id));


        }


        [HttpGet]
        public IActionResult GetAdvisementsByUser([FromQuery] string email)
        {

            return Json(advisementDAO.GetByUser(email));

        }


        [HttpGet]
        public IActionResult GetPublicAdvisements()
        {

            return Json(advisementDAO.GetPublicAdvisements());

        }



    }
}
