using Microsoft.AspNetCore.Mvc;
using TrainingApp.Application.Services.Implementation;
using TrainingApp.Application.Services.Interface;
using TrainingApp.Shared.DTOs.RequestDTOs;

namespace TrainingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService courseService) : ControllerBase
    {
        [HttpPost("create-courses")]
        public IActionResult CreateCourses(List<CourseRequestDTO> courses)
        {
            var result = courseService.CreateCourses(courses);
            return Ok(result);
        }

        [HttpGet("get-courses")]
        public IActionResult GetCourses()
        {
            try
            {
                var result = courseService.GetCourses();
                return Ok(result);
            } catch (Exception ex) { return Ok(ex); }
        }

        [HttpGet("get-scores")]
        public IActionResult GetScores()
        {
            var result = courseService.GetPersonCourse();
            return Ok(result);
        }
    }
}
