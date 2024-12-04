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
            dbContext = new AppDbContext();
            courseService = new CourseService(dbContext);        
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
        public void CreateCourses_DuplicateCourseId_ReturnsFailedResponse()
        {
            var expected = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "c11", CourseName = "Course One" },
                new CourseRequestDTO { CourseId = "c11", CourseName = "Duplicate Course" }
            };

            var actual = courseService.CreateCourses(expected);

            Assert.False(actual.Succeeded);
            Assert.Contains("Duplicate courseId found in input", actual.Message);
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

        [Fact]
        public void CreateCourses_ValidCourses_ReturnsSuccessResponse()
        {

            var expected = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "c11", CourseName = "Course One" },
                new CourseRequestDTO { CourseId = "c12", CourseName = "Course Two" }
            };

            var actual = courseService.CreateCourses(expected);

            Assert.True(actual.Succeeded);
            Assert.Contains("Courses successfully created", actual.Message);
            Assert.Equal(2, actual.Data.Count);
            Assert.Contains(actual.Data, c => c.CourseId == "c11" && c.CourseName == "Course One");
            Assert.Contains(actual.Data, c => c.CourseId == "c12" && c.CourseName == "Course Two");
        }
    }
}
