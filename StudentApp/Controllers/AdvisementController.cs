using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentApp.Models.DTO;
using StudentApp.Models.DAO;

namespace StudentApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdvisementController : Controller
    {
        private readonly ILogger<AdvisementController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string PROFESSOR_API_URL;

        AdvisementDAO advisementDAO;

        public AdvisementController(ILogger<AdvisementController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            advisementDAO = new AdvisementDAO(_configuration);

            PROFESSOR_API_URL = _configuration["EnvironmentVariables:PROFESSOR_API_URL"];

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateNewAdvisement([FromBody] CreateAdvisementDTO advisement)
        {
            try
            {
                advisement.Id = Guid.NewGuid().ToString();
                advisement.CreatedAt = DateTime.UtcNow;

                //TODO: Llamar API profesor:

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PROFESSOR_API_URL);

                    var postTask = client.PostAsJsonAsync("/api/Advisement/AddAdvisement", advisement);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Advisement", Error = errorMessage });
                    }
                }

                    return Ok(advisementDAO.Create(advisement));
            }
            catch (SqlException e)
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
        public IActionResult GetAdvisementById([FromQuery] string id)
        {
            try
            {
                return Ok(advisementDAO.GetById(id));
            }
            catch (SqlException e)
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
        public IActionResult GetAdvisementsByUser([FromQuery] string email)
        {
            try
            {
                return Json(advisementDAO.GetByUser(email));
            }
            catch (SqlException e)
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

        public IActionResult GetPublicAdvisements([FromQuery] string email)
        {
            try
            {
                return Json(advisementDAO.GetPublicAdvisements(email));
            }
            catch (SqlException e)
            {
                return StatusCode(500, new { message = "An error occurred", error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = e.Message });
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetAdvisementResponsesById(string id)
        {
            try
            {
                return Ok(advisementDAO.GetAdvisementResponsesById(id));
            }
            catch (SqlException e)
            {
                return StatusCode(500, new { message = "An error occurred", error = e.Message });
            }
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewResponse([FromBody] CreateResponseAdvisementDTO response)
        {
            try
            {

                response.Id = Guid.NewGuid().ToString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PROFESSOR_API_URL);

                    var responseFormat = new {
                            Id = response.Id,
                            advisementId = response.AdvisementId, // ID del asesoramiento al que responde
                            userId = response.UserId, // ID del usuario que responde
                            text = response.Text // Contenido de la respuesta
                    };


                    var postTask = client.PostAsJsonAsync("/api/Advisement/CreateResponseAdvisementFromStudent", responseFormat);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Advisement Response", Error = errorMessage });
                    }
                }


                return Ok(advisementDAO.InsertNewResponse(response));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewResponseFromAPI([FromBody] CreateResponseAdvisementDTO response)
        {
            try
            {
                return Ok(advisementDAO.InsertNewResponse(response));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
        }

    }
}
