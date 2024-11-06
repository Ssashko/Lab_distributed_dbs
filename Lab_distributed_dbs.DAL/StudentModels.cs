using MongoDB.Bson.Serialization.Attributes;

namespace Lab_distributed_dbs.DAL
{

    public class Group
    {
        [BsonId]
        [BsonElement("_id")]
        public string Name { get; set; }
        public string Specialization { get; set; }

        [BsonIgnore]
        public ICollection<Student> Students { get; set; }
    }

    public class Student
    {
        [BsonId]
        [BsonElement("_id")]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupName { get; set; }
        public Group Group { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

    public class Lecturer
    {
        [BsonId]
        [BsonElement("_id")]
        public int LecturerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Course> Courses { get; set; }

    }

    public class Course
    {
        [BsonId]
        [BsonElement("_id")]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int LecturerId { get; set; }
        [BsonIgnore]
        public Lecturer Lecturer { get; set; }
        [BsonIgnore]
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

    public class StudentCourse
    {
        [BsonId]
        [BsonElement("_id")]
        public int StudentId { get; set; }
        [BsonIgnore]
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }

}
