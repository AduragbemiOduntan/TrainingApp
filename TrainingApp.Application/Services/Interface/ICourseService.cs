using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Interface
{
    public interface ICourseService
    {
        List<CourseResponseDTO> CreateCourses(List<CourseRequestDTO> courses);
        List<CourseResponseDTO> GetCourses();
        List<PersonCourseRequestDTO> GetPersonCourse();

        
    }
}
