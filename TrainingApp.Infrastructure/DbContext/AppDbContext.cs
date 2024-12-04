using TrainingApp.Domain.Models;

namespace TrainingApp.Infrastructure.DbContext
{
    public class AppDbContext
    {
        public readonly List<Course> Courses = new List<Course>
        {
            new Course { CourseId = "c1", CourseName = "Accounting 101" },
            new Course { CourseId = "c2", CourseName = "Marketing 101" },
            new Course { CourseId = "c3", CourseName = "Entrepreneurship Basics" },
            new Course { CourseId = "c4", CourseName = "Business Law" },
            new Course { CourseId = "c5", CourseName = "Financial Planning" },
            new Course { CourseId = "c6", CourseName = "Leadership Development" },
            new Course { CourseId = "c7", CourseName = "Strategic Management" },
            new Course { CourseId = "c8", CourseName = "Sales Techniques" },
            new Course { CourseId = "c9", CourseName = "Digital Marketing" },
            new Course { CourseId = "c10", CourseName = "Supply Chain Management" }

        };
        public readonly List<Person> Persons = new List<Person>();
        public readonly List<PersonCourse> PersonCourses = new List<PersonCourse>();
    }
}
