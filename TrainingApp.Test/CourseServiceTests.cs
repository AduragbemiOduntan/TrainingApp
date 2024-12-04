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
            var expected = "Input cannot be null";
            List<CourseRequestDTO> data = null;
            var actual = courseService.CreateCourses(data);

            Assert.False(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
            Assert.Equal(99, actual.StatusCode);
        }

        [Fact]
        public void CreateCourses_EmptyInput_ReturnsFailedResponse()
        {
            var expected = "Course data is required";
            var data = new List<CourseRequestDTO>();
            var actual = courseService.CreateCourses(data);

            Assert.False(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
            Assert.Equal(99, actual.StatusCode);
        }

        [Fact]
        public void CreateCourses_InvalidCourseId_ReturnsFailedResponse()
        {
            var expected = "Invalid course id";
            var data = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "x1", CourseName = "Invalid Course" }
            };

            var actual = courseService.CreateCourses(data);

            Assert.False(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
        }

        [Fact]
        public void CreateCourses_DuplicateCourseId_ReturnsFailedResponse()
        {
            var expected = "Duplicate courseId found in input";
            var data = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "c11", CourseName = "Course One" },
                new CourseRequestDTO { CourseId = "c11", CourseName = "Duplicate Course" }
            };

            var actual = courseService.CreateCourses(data);

            Assert.False(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
        }

        [Fact]
        public void CreateCourses_ExistingCourseId_ReturnsFailedResponse()
        {
            var expected = "Course with courseId: c1 already exists";
            var data = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "c1", CourseName = "New Course With Existing ID" }
            };

            var actual = courseService.CreateCourses(data);

            Assert.False(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
        }

        [Fact]
        public void CreateCourses_ValidCourses_ReturnsSuccessResponse()
        {
            var expected = "Courses successfully created";
            var data = new List<CourseRequestDTO>
            {
                new CourseRequestDTO { CourseId = "c11", CourseName = "Course One" },
                new CourseRequestDTO { CourseId = "c12", CourseName = "Course Two" }
            };

            var actual = courseService.CreateCourses(data);

            Assert.True(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
            Assert.Equal(2, actual.Data.Count);
            Assert.Contains(actual.Data, c => c.CourseId == "c11" && c.CourseName == "Course One");
            Assert.Contains(actual.Data, c => c.CourseId == "c12" && c.CourseName == "Course Two");
        }
    }
}
