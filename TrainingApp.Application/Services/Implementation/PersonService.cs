using TrainingApp.Application.Services.Interface;
using TrainingApp.Domain.Models;
using TrainingApp.Infrastructure.DbContext;
using TrainingApp.Shared.DTOs.RequestDTOs;
using TrainingApp.Shared.DTOs.ResponseDTOs;

namespace TrainingApp.Application.Services.Implementation
{
    public class PersonService(AppDbContext dbContext) : IPersonService
    {
        public StandardResponse<List<PersonResponseDTO>> CreatePersons(List<PersonRequestDTO> persons)
        {
            if (persons == null)
            {
                return StandardResponse<List<PersonResponseDTO>>.Failed("Input cannot be null");
            }

            if (persons.Count == 0)
            {
                return StandardResponse<List<PersonResponseDTO>>.Failed("Persons data is required");
            }

            var errors = new List<string>();
            var addedPersons = new List<PersonResponseDTO>();

            foreach (var person in persons)
            {
                if (string.IsNullOrWhiteSpace(person.PersonId) || string.IsNullOrWhiteSpace(person.CourseId) || string.IsNullOrWhiteSpace(person.Name))
                {
                    errors.Add($"Missing required field for person");
                    continue;
                }

                if (!person.PersonId.StartsWith("e", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"Invalid person id: {person.CourseId}. person id must start with 'e'.");
                    continue;
                }

                if (dbContext.PersonCourses.Any(pc => pc.PersonId == person.PersonId && pc.CourseId == person.CourseId))
                {
                    errors.Add($"Duplicate entry for person course id: {person.PersonId} and courseId: {person.CourseId}");
                    continue;
                }

                var newPerson = new Person
                {
                    PersonId = person.PersonId,
                    Name = person.Name,
                };
                dbContext.Persons.Add(newPerson);

                var newPersonCourse = new PersonCourse
                {
                    PersonCourseId = $"pc{dbContext.PersonCourses.Count + 1}",
                    CourseId = person.CourseId,
                    PersonId = person.PersonId,
                    Score = person.Score,
                };
                dbContext.PersonCourses.Add(newPersonCourse);
                addedPersons.Add(new PersonResponseDTO { PersonId = newPerson.PersonId, CourseId = person.CourseId, Name = newPerson.Name, Score = person.Score });

            }
            if (errors.Count > 0)
            {
                return StandardResponse<List<PersonResponseDTO>>.Failed(string.Join("; ", errors));
            }
            return StandardResponse<List<PersonResponseDTO>>.Success("Persons successfully created", addedPersons);
        }

        public StandardResponse<PersonProgressResponseDTO> GetPersonProgressById(string personId)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(personId))
                {
                    return StandardResponse<PersonProgressResponseDTO>.Failed("Person id cannot be null");
                }
                var person = dbContext.Persons.FirstOrDefault(p => p.PersonId.Equals(personId));
                if (person == null)
                {
                    return StandardResponse<PersonProgressResponseDTO>.Failed($"Person with id: {personId} does not exist");
                }
                var personCourses = dbContext.PersonCourses.Where(pc => pc.PersonId.Equals(personId)).ToList();
                var personProgress = new PersonProgressResponseDTO
                {
                    PersonName = person.Name,
                };

                double totalScore = 0; double count = 0;
                foreach (var personCourse in personCourses)
                {
                    var course = dbContext.Courses.FirstOrDefault(c => c.CourseId.Equals(personCourse.CourseId));
                    if (course == null )
                    {
                        return StandardResponse<PersonProgressResponseDTO>.Success($"No course progress for person with id: {personId}", new PersonProgressResponseDTO());
                    }
                    if (course != null)
                    {
                        var courseScore = new CourseScoreResponseDTO
                        {
                            CourseName = course.CourseName,
                            Score = personCourse.Score,
                        };
                        personProgress.CourseScore.Add(courseScore);
                        totalScore += courseScore.Score;
                        count++;
                    }
                }
                if (totalScore > 0)
                {
                    personProgress.GradePointAverage = totalScore / count;
                }
                else
                {
                    personProgress.GradePointAverage = 0;
                }

                return StandardResponse<PersonProgressResponseDTO>.Success("Person progress successfully retrieved", personProgress);
            }
            catch (Exception ex)
            {
                return StandardResponse<PersonProgressResponseDTO>.Failed($"Error: {ex.Message}");
            }
        }

        public StandardResponse<PersonResponseDTO> GetPersonById(string personId)
        {
            throw new NotImplementedException();
        }

        public StandardResponse<List<PersonResponseDTO>> GetPersons()
        {
            throw new NotImplementedException();
        }
    }

}

