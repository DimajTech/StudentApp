using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentApp.Models.DAO;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;

namespace StudentApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentNewsController : Controller
	{
		private readonly ILogger<CommentNewsController> _logger;
		private readonly IConfiguration _configuration;
        CommentNewsDAO commentNewsDAO;
        CommentNewsResponseDAO commentNewsResponseDAO;
        PieceOfNewsDAO newsDAO;

        private readonly string PROFESSOR_API_URL;
        private readonly string ADMIN_API_URL;

        public CommentNewsController(ILogger<CommentNewsController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;

            commentNewsDAO = new CommentNewsDAO(_configuration);
            newsDAO = new PieceOfNewsDAO(_configuration);
            commentNewsResponseDAO = new CommentNewsResponseDAO(_configuration);


            PROFESSOR_API_URL = _configuration["EnvironmentVariables:PROFESSOR_API_URL"];
            ADMIN_API_URL = _configuration["EnvironmentVariables:ADMIN_API_URL"];

        }


        [HttpGet]
        [Route("[action]/{id}")]
        public IEnumerable<CommentNews> GetCommentNewsByPieceOfNewsId(string id)
        {
            return commentNewsDAO.GetCommentsByPieceOfNewsId(id);
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IEnumerable<CommentNewsResponse> GetCommentNewsResponsesByCommentId(string id)
        {
            return commentNewsResponseDAO.GetResponsesByCommentNewsId(id);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewsComment([FromBody] CommentNewsDTO comment)
		{
            try
            {
                //int result = commentNewsDAO.Insert(comment);

                CommentNews newComment = new CommentNews();
                newComment.User = new User();
                newComment.PieceOfNews = new PieceOfNews();

                newComment.Id = comment.Id != null ? comment.Id : Guid.NewGuid().ToString();
                newComment.User.Id = comment.AuthorId;
                newComment.PieceOfNews.Id = comment.PieceOfNewsId;
                newComment.Text = comment.Text;

                comment.Id = newComment.Id;

                //Se llama a la API de profesores para registrar el comentario:
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PROFESSOR_API_URL);


                    var postTask = client.PostAsJsonAsync("/api/CommentNews/AddNewsCommentFromMVC", comment);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                    }
                    else
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Comment", Error = errorMessage });
                    }

                }

                //Se llama a la API admins:


                using (var client = new HttpClient())
                {

                    //Comunicarse con admin
                    client.BaseAddress = new Uri(ADMIN_API_URL);


                    var postTask = client.PostAsJsonAsync("/api/commentNews/saveCommentNews", comment);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Comment", Error = errorMessage });
                    }

                }

                return Ok(commentNewsDAO.Insert(newComment));

            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
		}


        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewsCommentResponse([FromBody] CommentNewsResponseDTO commentResponse)
        {
            try
            {
                //int result = commentNewsDAO.Insert(comment);

                CommentNewsResponse newCommentResponse = new CommentNewsResponse();
                newCommentResponse.User = new User();
                newCommentResponse.CommentNews = new CommentNews();

                newCommentResponse.Id = commentResponse.Id != null ? commentResponse.Id : Guid.NewGuid().ToString();
                newCommentResponse.User.Id = commentResponse.AuthorId;
                newCommentResponse.CommentNews.Id = commentResponse.CommentNewsId;
                newCommentResponse.Text = commentResponse.Text;


                commentResponse.Id = newCommentResponse.Id;


                //Se llama a la API de profesores para registrar el comentario:
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PROFESSOR_API_URL);


                    var postTask = client.PostAsJsonAsync("/api/CommentNews/AddNewsCommentResponseFromMVC", commentResponse);
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

                //Se llama a la API admins:


                using (var client = new HttpClient())
                {

                    //Comunicarse con admin
                    client.BaseAddress = new Uri(ADMIN_API_URL);


                    var postTask = client.PostAsJsonAsync("/api/commentNewsResponse/saveCommentNewsResponse", commentResponse);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (!result.IsSuccessStatusCode)
                    {
                        var errorMessage = result.Content.ReadAsStringAsync().Result;
                        return StatusCode((int)result.StatusCode, new { Message = "Failed to add Response", Error = errorMessage });
                    }

                }

                return Ok(commentNewsResponseDAO.Insert(newCommentResponse));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
        }

        //---------------Métodos para las APIs----------------\\
        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewsCommentFromAPI([FromBody] CommentNewsDTO comment)
        {
            try
            {
                //int result = commentNewsDAO.Insert(comment);

                CommentNews newComment = new CommentNews();
                newComment.User = new User();
                newComment.PieceOfNews = new PieceOfNews();

                newComment.Id = comment.Id != null ? comment.Id : Guid.NewGuid().ToString();
                newComment.User.Id = comment.AuthorId;
                newComment.PieceOfNews.Id = comment.PieceOfNewsId;
                newComment.Text = comment.Text;

                return Ok(commentNewsDAO.Insert(newComment));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewsCommentResponseFromAPI([FromBody] CommentNewsResponseDTO commentResponse)
        {
            try
            {
                //int result = commentNewsDAO.Insert(comment);

                CommentNewsResponse newCommentResponse = new CommentNewsResponse();
                newCommentResponse.User = new User();
                newCommentResponse.CommentNews = new CommentNews();

                newCommentResponse.Id = commentResponse.Id != null ? commentResponse.Id : Guid.NewGuid().ToString();
                newCommentResponse.User.Id = commentResponse.AuthorId;
                newCommentResponse.CommentNews.Id = commentResponse.CommentNewsId;
                newCommentResponse.Text = commentResponse.Text;

                return Ok(commentNewsResponseDAO.Insert(newCommentResponse));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
        }


        [HttpPost]
        [Route("[action]/{id}")]
        public IActionResult DeleteCommentNewsById(string id)
        {
            try
            {
                return Ok(commentNewsDAO.DeleteCommentNewsById(id));
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
        [Route("[action]/{id}")]
        public IActionResult DeleteResponseById(string id)
        {
            try
            {
                return Ok(commentNewsDAO.DeleteResponseById(id));
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
