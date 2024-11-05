namespace Lab_distributed_dbs.DAL
{

    public class Group
    { 
        public string Name { get; set; }
        public string Specialization { get; set; }
        public ICollection<Student> Students { get; set; }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupName { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

    public class StudentCourse
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }

}
