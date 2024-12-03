using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Application.Services.Interface;
using TrainingApp.Infrastructure.DbContext;
using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Implementation
{
    public class CourseService(AppDbContext dbContext) : ICourseService
    {
        public List<CourseResponseDTO> CreateCourses(List<CourseRequestDTO> courses)
        {
            throw new NotImplementedException();
        }

        public List<CourseResponseDTO> GetCourses()
        {
            throw new NotImplementedException();
        }

        public List<PersonCourseRequestDTO> GetPersonCourse()
        {
            throw new NotImplementedException();
        }
    }
}
