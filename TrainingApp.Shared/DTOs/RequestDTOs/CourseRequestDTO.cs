using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingApp.Shared.DTOs.RequestDTOs
{
    public class CourseRequestDTO
    {
        [Required(ErrorMessage = "Course ID is required")]
        public string CourseId { get; set; }
        [Required (ErrorMessage = "Course name is required")]
        public string CourseName { get; set; }
    }
}
