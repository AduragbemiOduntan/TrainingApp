using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Interface
{
    public interface ICourseService
    {
        StandardResponse<List<CourseResponseDTO>> CreateCourses(List<CourseRequestDTO> courses);
        StandardResponse<List<CourseResponseDTO>> GetCourses();
        StandardResponse<List<PersonCourseRequestDTO>> GetPersonCourse();

        
    }
}
