using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentApp.Models.DAO;
using StudentApp.Models.Entity;
using System.IO.Pipelines;

namespace StudentApp.Controllers
{
    public class PieceOfNewsController : Controller
    {
        private readonly ILogger<PieceOfNewsController> _logger;
        private readonly IConfiguration _configuration;
        PieceOfNewsDAO newsDAO;

        public PieceOfNewsController(ILogger<PieceOfNewsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            newsDAO = new PieceOfNewsDAO(_configuration);

        }
        [HttpGet]
        public IEnumerable<PieceOfNews> GetNews()
        {
            //TODO Manejar excepciones


            return newsDAO.Get();
        }

        public PieceOfNews GetById(string id)
        {

            return newsDAO.Get(id);
        }
    }
}
