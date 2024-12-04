namespace TrainingApp.Shared.DTOs.ResponseDTOs
{
    public class PersonProgressResponseDTO
    {
        public string PersonName { get; set; }
        public List<CourseScoreResponseDTO> CourseScore { get; set; }
        public double GradePointAverage { get; set; }

        public PersonProgressResponseDTO()
        {
            CourseScore = new List<CourseScoreResponseDTO>();
        }
    }
}
