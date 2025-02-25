using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentApp.Models.DAO;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;
using System.IO.Pipelines;

namespace StudentApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PieceOfNewsController : Controller
    {
        private readonly ILogger<PieceOfNewsController> _logger;
        private readonly IConfiguration _configuration;
        PieceOfNewsDAO newsDAO;
        private readonly string PROFESSOR_API_URL;
        private readonly string ADMIN_API_URL;

        public PieceOfNewsController(ILogger<PieceOfNewsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            newsDAO = new PieceOfNewsDAO(_configuration);
            PROFESSOR_API_URL = _configuration["EnvironmentVariables:PROFESSOR_API_URL"];
            ADMIN_API_URL = _configuration["EnvironmentVariables:ADMIN_API_URL"];

        }


        [HttpGet]
        [Route("[action]")]
        public IEnumerable<PieceOfNews> GetNews()
        {
            //TODO Manejar excepciones


            return newsDAO.Get();
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateNews([FromBody] CreatePieceOfNewsDTO news)
        {
            try
            {
                //Mandar la noticia a la API profesores
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PROFESSOR_API_URL);

                    news.Id = Guid.NewGuid().ToString();
                    var postTask = client.PostAsJsonAsync("/api/PieceOfNews/AddPieceOfNews", news);
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

                using (var client = new HttpClient())
                {

                    //Comunicarse con admin
                    client.BaseAddress = new Uri(ADMIN_API_URL);

                    news.AuthorId = news.UserId;

                    var postTask = client.PostAsJsonAsync("/api/pieceOfNews/savePieceOfNews", news);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Response", Error = errorMessage });
                    }

                }

                return Ok(newsDAO.Insert(news));

            }
            catch (SqlException e)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred",
                    error = e.Message
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = e.Message });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateNewsFromAdmin([FromBody] CreatePieceOfNewsDTO news)
        {
            try
            {
              return Ok(newsDAO.Insert(news));
            }
            catch (SqlException e)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred",
                    error = e.Message
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = e.Message });
            }
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                PieceOfNews news =  newsDAO.Get(id);

                Console.WriteLine(news);
                if(news.User is null)
                {
                    return Ok(null);

                }
                return Ok(news);

            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = e.Message });

            }
        }


        [HttpPost]
        [Route("[action]/{id}")]
        public IActionResult DeletePieceOfNews(string id)
        {
            try
            {
                return Ok(newsDAO.Delete(id));
            }
            catch (SqlException e)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred",
                    error = e.Message
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = e.Message });
            }
        }
    }
}
