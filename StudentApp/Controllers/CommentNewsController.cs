using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentApp.Models.DAO;
using StudentApp.Models.Entity;

namespace StudentApp.Controllers
{
	public class CommentNewsController : Controller
	{
		private readonly ILogger<CommentNewsController> _logger;
		private readonly IConfiguration _configuration;
		CommentNewsDAO commentNewsDAO;

		public CommentNewsController(ILogger<CommentNewsController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;

			commentNewsDAO = new CommentNewsDAO(_configuration);

		}


		public IEnumerable<CommentNews> GetCommentNewsByPieceOfNewsId(string id)
		{
			return commentNewsDAO.GetCommentsByPieceOfNewsId(id);
		}

		public IActionResult AddNewsComment([FromBody] CommentNews comment)
		{
			try
			{
				//int result = commentNewsDAO.Insert(comment);

				return Ok(commentNewsDAO.Insert(comment));
			}
			catch (SqlException e)
			{
				ViewBag.Message = e.Message;
				return StatusCode(500, new { message = "An error ocurred", error = e.Message });
			}
		}



	}
}
