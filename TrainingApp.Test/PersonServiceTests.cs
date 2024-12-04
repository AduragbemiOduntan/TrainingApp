using TrainingApp.Application.Services.Implementation;
using TrainingApp.Domain.Models;
using TrainingApp.Infrastructure.DbContext;
using TrainingApp.Shared.DTOs.RequestDTOs;
using Xunit;

namespace TrainingApp.Test
{
    public class PersonServiceTests
    {
        private readonly AppDbContext dbContext;
        private readonly PersonService personService;

        public PersonServiceTests()
        {
            dbContext = new AppDbContext();
            personService = new PersonService(dbContext);
        }

        [Fact]
        public void CreatePersons_ShouldReturnFailed_WhenPersonsIsNull()
        {
            var expected = "Input cannot be null";
            List<PersonRequestDTO> data = null;
            var actual = personService.CreatePersons(data);

            Assert.False(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
        }

        [Fact]
        public void CreatePersons_ShouldReturnFailed_WhenPersonsIsEmpty()
        {
            var expected = "Persons data is required";
            List<PersonRequestDTO> data = new();
            var actual = personService.CreatePersons(data);

            Assert.False(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
        }

        [Fact]
        public void CreatePersons_ShouldReturnFailed_WhenRequiredFieldsAreMissing()
        {
            var expected = "Missing required field for person";
            var data = new List<PersonRequestDTO>
            {
                new PersonRequestDTO { PersonId = "", CourseId = "c1", Name = "John" },
                new PersonRequestDTO { PersonId = "e123", CourseId = "", Name = "Jane" }
            };

            var actual = personService.CreatePersons(data);

            Assert.False(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
        }

        [Fact]
        public void CreatePersons_ShouldReturnFailed_WhenPersonIdDoesNotStartWithE()
        {
            var expected = "Invalid person id: c1. person id must start with 'e'.";
            var data = new List<PersonRequestDTO>
            {
                new PersonRequestDTO { PersonId = "a123", CourseId = "c1", Name = "John" }
            };

            var actual = personService.CreatePersons(data);

            Assert.False(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
        }

        [Fact]
        public void CreatePersons_ShouldReturnFailed_WhenDuplicatePersonCourseExists()
        {
            var expected = "Duplicate entry for person course id: e123 and courseId: c1";
            dbContext.PersonCourses.Add(new PersonCourse { PersonId = "e123", CourseId = "c1" });

            var data = new List<PersonRequestDTO>
            {
                new PersonRequestDTO { PersonId = "e123", CourseId = "c1", Name = "John" }
            };

            var actual = personService.CreatePersons(data);

            Assert.False(actual.Succeeded);
            Assert.Contains(expected, actual.Message);
        }

        [Fact]
        public void CreatePersons_ShouldReturnSuccess_WhenValidPersonsAreProvided()
        {
            var expected = "Persons successfully created";
            var expectedDataCount = 2;

            var datas = new List<PersonRequestDTO>
            {
                new PersonRequestDTO { PersonId = "e123", CourseId = "c1", Name = "John", Score = 5 },
                new PersonRequestDTO { PersonId = "e124", CourseId = "c2", Name = "Jane", Score = 4 }
            };

            var actual = personService.CreatePersons(datas);

            Assert.True(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
            Assert.Equal(expectedDataCount, actual.Data.Count);
        }

        //-------------------------------- Get  Progress Test

        [Fact]
        public void GetPersonProgressById_ShouldReturnFailed_WhenPersonIdIsNull()
        {
            var expected = "Person id cannot be null";
            var actual = personService.GetPersonProgressById(null);

            Assert.False(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
        }

        [Fact]
        public void GetPersonProgressById_ShouldReturnFailed_WhenPersonDoesNotExist()
        {
            var expected = "Person with id: e999 does not exist";
            var actual = personService.GetPersonProgressById("e999");

            Assert.False(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
        }

        [Fact]
        public void GetPersonProgressById_ShouldReturnSuccess_WhenPersonHasNoCourses()
        {
            var expected = "Person progress successfully retrieved";
            var expectedPersonName = "John Doe";

            var data = new Person { PersonId = "e123", Name = "John Doe" };
            dbContext.Persons.Add(data);

            var actual = personService.GetPersonProgressById("e123");

            Assert.True(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
            Assert.Equal(expectedPersonName, actual.Data.PersonName);
            Assert.Empty(actual.Data.CourseScore);
        }

        [Fact]
        public void GetPersonProgressById_ShouldReturnSuccess_WhenPersonHasCourses()
        {
            var expected = "Person progress successfully retrieved";
            var expectedCourseScoreCount = 4;
            var expectedGradePointAverage = 100;

            dbContext.Persons.Add(new Person { PersonId = "e123", Name = "Peter" });
            var datas = new List<PersonCourse>
            {
                new PersonCourse{PersonCourseId = "pc1", CourseId = "c1", PersonId = "e123", Score = 100},
                new PersonCourse{PersonCourseId = "pc2", CourseId = "c2", PersonId = "e123", Score = 100},
                new PersonCourse{PersonCourseId = "pc3", CourseId = "c2", PersonId = "e123", Score = 100},
                new PersonCourse{PersonCourseId = "pc4", CourseId = "c2", PersonId = "e123", Score = 100},
            };
            foreach (var data in datas)
            {
                dbContext.PersonCourses.Add(data);
            }

            var actual = personService.GetPersonProgressById("e123");

            Assert.True(actual.Succeeded);
            Assert.Equal(expected, actual.Message);
            Assert.Equal(expectedCourseScoreCount, actual.Data.CourseScore.Count);
            Assert.Equal(expectedGradePointAverage, actual.Data.GradePointAverage);
        }
    }
}
