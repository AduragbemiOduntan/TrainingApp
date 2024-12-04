using TrainingApp.Application.Services.Implementation;
using TrainingApp.Domain.Models;
using TrainingApp.Infrastructure.DbContext;
using TrainingApp.Shared.DTOs.RequestDTOs;
using Xunit;

namespace TrainingApp.Test
{
    public class CourseServiceTests
    {
        private readonly CourseService courseService;
        private readonly AppDbContext dbContext ;

        public CourseServiceTests()
        {
            courseService = new CourseService(new AppDbContext());
            dbContext = new AppDbContext();
        }


        [Fact]
        public void CreateCourses_NullInput_ReturnsFailedResponse()
        {
            List<CourseRequestDTO> expected = null;
            var actual = courseService.CreateCourses(expected);

            Assert.False(actual.Succeeded);
            Assert.Equal("Input cannot be null", actual.Message);
            Assert.Equal(99, actual.StatusCode);
        }

        [Fact]
        public void CreateCourses_EmptyInput_ReturnsFailedResponse()
        {
            var expected = new List<CourseRequestDTO>();
            var actual = courseService.CreateCourses(expected);

            Assert.False(actual.Succeeded);
            Assert.Equal("Course data is required", actual.Message);
            Assert.Equal(99, actual.StatusCode);
        }

        [Fact]
        public void CreateCourses_InvalidCourseId_ReturnsFailedResponse()
        {          
            var expected = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "x1", CourseName = "Invalid Course" }
            };

            var actual = courseService.CreateCourses(expected);

            Assert.False(actual.Succeeded);
            Assert.Contains("Invalid course id", actual.Message);
        }

        [Fact]
        public void CreateCourses_ExistingCourseId_ReturnsFailedResponse()
        {
          
            dbContext.Courses.Add(new Course { CourseId = "c1", CourseName = "Existing Course" });

            var expected = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "c1", CourseName = "New Course With Existing ID" }
            };

            var actual = courseService.CreateCourses(expected);

            Assert.False(actual.Succeeded);
            Assert.Contains("Course with courseId: c1 already exists", actual.Message);
        }
    }
}
