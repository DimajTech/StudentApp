using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentApp.Models.DAO;
using StudentApp.Models.DTO;
using StudentApp.Models.Entity;

namespace StudentApp.Controllers
{
    public class CourseController : Controller
    {


        private readonly ILogger<CourseController> _logger;
        private readonly IConfiguration _configuration;

        CourseDAO courseDAO;

        public CourseController(ILogger<CourseController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            courseDAO = new CourseDAO(_configuration);
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult GetAllCourses()
        {
           
            return Ok(courseDAO.Get());

        }

        public IActionResult GetCourseByCode(string code)
        {
          
            return Ok(courseDAO.GetByCode(code));

        }

        [HttpPost]
        public IActionResult PostCourse([FromBody] CourseCrudDTO course)
        {
            try
            {
                return Ok(courseDAO.PostCourse(course));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteCourse(string id)
        {
            try
            {
                return Ok(courseDAO.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult PutCourse(string id, [FromBody] CourseCrudDTO course)
        {
            try
            {
                if(id  == null || id != course.id)
                {
                    return BadRequest();
                }


                return Ok(courseDAO.Update(course));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
