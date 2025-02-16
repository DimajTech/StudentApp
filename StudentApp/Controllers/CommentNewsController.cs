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
        public CommentNewsController(ILogger<CommentNewsController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;

            commentNewsDAO = new CommentNewsDAO(_configuration);
            newsDAO = new PieceOfNewsDAO(_configuration);
            commentNewsResponseDAO = new CommentNewsResponseDAO(_configuration);

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

                return Ok(commentNewsResponseDAO.Insert(newCommentResponse));
            }
            catch (SqlException e)
            {
                ViewBag.Message = e.Message;
                return StatusCode(500, new { message = "An error ocurred", error = e.Message });
            }
        }



    }
}
