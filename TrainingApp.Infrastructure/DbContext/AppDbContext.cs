using TrainingApp.Domain.Models;

namespace TrainingApp.Infrastructure.DbContext
{
    public class AppDbContext
    {
        public readonly List<Person> Persons = new ();
        public readonly List<Course> Courses = new ();
        public readonly List<PersonCourse> PersonCourses = new ();
    }
}
