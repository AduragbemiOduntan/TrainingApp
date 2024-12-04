using TrainingApp.Application.Services.Interface;
using TrainingApp.Domain.Models;
using TrainingApp.Infrastructure.DbContext;
using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Implementation
{
    public class CourseService(AppDbContext dbContext) : ICourseService
    {
        public StandardResponse<List<CourseResponseDTO>> CreateCourses(List<CourseRequestDTO> courses)
        {
            try
            {
                if (courses == null)
                {
                    return StandardResponse<List<CourseResponseDTO>>.Failed("Input cannot be null");
                }
                if (courses.Count == 0)
                {
                    return StandardResponse<List<CourseResponseDTO>>.Failed("Course data is required");
                }

                var addedCourses = new List<CourseResponseDTO>();
                var errors = new List<string>();
                var existingCourseIds = dbContext.Courses.Select(c => c.CourseId).ToHashSet();
                var processedIds = new HashSet<string>();
                foreach (var course in courses)
                {
                    if (string.IsNullOrWhiteSpace(course.CourseId) || string.IsNullOrWhiteSpace(course.CourseName))
                    {
                        errors.Add($"Missing required fields for courseId: {course.CourseId ?? "null"}");
                        continue;
                    }

                    if (!course.CourseId.StartsWith("c", StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Invalid course id: {course.CourseId}. Course id must start with 'c'.");
                        continue;
                    }

                    if (processedIds.Contains(course.CourseId))
                    {
                        errors.Add($"Duplicate courseId found in input: {course.CourseId}");
                        continue;
                    }

                    if (existingCourseIds.Contains(course.CourseId))
                    {
                        errors.Add($"Course with courseId: {course.CourseId} already exists");
                        continue;
                    }
                    var newCourse = new Course
                    {
                        CourseId = course.CourseId.ToLower(),
                        CourseName = course.CourseName,
                    };
                    dbContext.Courses.Add(newCourse);
                    addedCourses.Add(new CourseResponseDTO { CourseId = course.CourseId, CourseName = course.CourseName });
                    processedIds.Add(course.CourseId);
                }

                if (errors.Count > 0)
                {
                    return StandardResponse<List<CourseResponseDTO>>.Failed($"Errors: {string.Join("; ", errors)}");
                }
                return StandardResponse<List<CourseResponseDTO>>.Success("Courses successfully created", addedCourses);
            }
            catch (Exception ex)
            {
                return StandardResponse<List<CourseResponseDTO>>.Failed($"Unexpected error occured: {ex.Message}");
            }
        }

        public StandardResponse<List<CourseResponseDTO>> GetCourses()
        {
            try
            {
                if (!dbContext.Courses.Any())
                {
                    return StandardResponse<List<CourseResponseDTO>>.Failed("No courses found");
                }
                var courses = new List<CourseResponseDTO>();
                foreach (var course in dbContext.Courses)
                {
                    var newCourse = new CourseResponseDTO
                    {
                        CourseId = course.CourseId,
                        CourseName = course.CourseName,
                    };
                    courses.Add(newCourse);
                }
                return StandardResponse<List<CourseResponseDTO>>.Success("Courses successfully retrieved", courses);
            }
            catch (Exception ex)
            {
                return StandardResponse<List<CourseResponseDTO>>.Failed($"Unexpected error occured: {ex.Message}");
            }
        }

        public StandardResponse<List<PersonCourseRequestDTO>> GetPersonCourse()
        {
            try
            {
                if (!dbContext.PersonCourses.Any())
                {
                    return StandardResponse<List<PersonCourseRequestDTO>>.Success("No person courses found", new List<PersonCourseRequestDTO>());
                }
                var personCourses = new List<PersonCourseRequestDTO>();
                foreach (var pc in dbContext.PersonCourses)
                {
                    var newPersonCourse = new PersonCourseRequestDTO
                    {
                        PersonCourseId = pc.PersonCourseId,
                        Score = pc.Score,
                        CourseId = pc.CourseId,
                        PersonId = pc.PersonId,
                    };
                    personCourses.Add(newPersonCourse);
                }
                return StandardResponse<List<PersonCourseRequestDTO>>.Success("Person Courses successfully retrieved", personCourses);
            }
            catch (Exception ex)
            {
                return StandardResponse<List<PersonCourseRequestDTO>>.Failed($"Unexpected error occured: {ex.Message}");
            }
        }
    }
}
