using System;
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

                //if (!dbContext.Courses.Any(c => c.CourseId == person.CourseId))
                //{
                //    errors.Add($"Invalid courseId: {person.CourseId}");
                //    continue;
                //}


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

        public PersonResponseDTO GetPersonById(string id)
        {
            throw new NotImplementedException();
        }

        public List<PersonResponseDTO> GetPersons()
        {
            throw new NotImplementedException();
        }
    }


    //public List<PersonResponseDTO> CreatePersons()
    //{
    //    throw new NotImplementedException();
    //}

    //public PersonResponseDTO GetPersonById(string id)
    //{
    //    throw new NotImplementedException();
    //}

    //public List<PersonResponseDTO> GetPersons()
    //{
    //    throw new NotImplementedException();
    //}
}

