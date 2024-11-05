using Lab_distributed_dbs.DAL;
using Lab_distributed_dbs.DAL.Settings;
using Lab_distributed_dbs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Lab_distributed_dbs
{
   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SqlController : ControllerBase
    {
        private readonly LabDbContext _context;
        public SqlController(LabDbContext context) => _context = context;

        [HttpPost]
        public IActionResult Fill() 
        {

            if (_context.Groups.Any() || _context.Students.Any() || _context.Lecturers.Any() || _context.Courses.Any())
            {
                return BadRequest("Database already contains data.");
            }

            var group1 = new Group { Name = "CS101", Specialization = "Computer Science" };
            var group2 = new Group { Name = "MAT101", Specialization = "Mathematics" };

            var student1 = new Student { FirstName = "Alice", LastName = "Johnson", Group = group1 };
            var student2 = new Student { FirstName = "Bob", LastName = "Smith", Group = group1 };
            var student3 = new Student { FirstName = "Charlie", LastName = "Brown", Group = group2 };

            var lecturer1 = new Lecturer { FirstName = "Dr. Emily", LastName = "Taylor" };
            var lecturer2 = new Lecturer { FirstName = "Prof. Daniel", LastName = "Anderson" };

            var course1 = new Course { Title = "Algorithms", Credits = 3, Lecturer = lecturer1 };
            var course2 = new Course { Title = "Linear Algebra", Credits = 4, Lecturer = lecturer2 };
            var course3 = new Course { Title = "Databases", Credits = 3, Lecturer = lecturer1 };

            var studentCourse1 = new StudentCourse { Student = student1, Course = course1 };
            var studentCourse2 = new StudentCourse { Student = student2, Course = course1 };
            var studentCourse3 = new StudentCourse { Student = student2, Course = course2 };
            var studentCourse4 = new StudentCourse { Student = student3, Course = course3 };
            _context.Groups.AddRange(group1, group2);
            _context.Students.AddRange(student1, student2, student3);
            _context.Lecturers.AddRange(lecturer1, lecturer2);
            _context.Courses.AddRange(course1, course2, course3);
            _context.StudentCourses.AddRange(studentCourse1, studentCourse2, studentCourse3, studentCourse4);

            _context.SaveChanges();

            return Ok("Database filled with sample data.");
        }

        [HttpDelete]
        public IActionResult Clear()
        {
            _context.StudentCourses.RemoveRange(_context.StudentCourses);
            _context.Students.RemoveRange(_context.Students);
            _context.Courses.RemoveRange(_context.Courses);
            _context.Lecturers.RemoveRange(_context.Lecturers);
            _context.Groups.RemoveRange(_context.Groups);

            _context.SaveChanges();

            return Ok("All data has been cleared from the database.");
        }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MongoController : ControllerBase
    {
        private readonly UpdateService _sync;
        private readonly StudentService _studentService;
        private readonly LecturerService _lecturerService;
        public MongoController(UpdateService sync, 
            StudentService studentService,
            LecturerService lecturerService)
        {
            _studentService = studentService;
            _sync = sync;
            _lecturerService = lecturerService;
        }
        private readonly IMongoCollection<Student> _studentsCollection;

        [HttpPut]
        public async Task<IActionResult> Update()
        {
            try
            {
                await _sync.Sync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok("All data has been synchronized.");
        }
        [HttpDelete]
        public async Task<IActionResult> Clear(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient
        mongoClient)
        {
            await mongoClient.DropDatabaseAsync(mongoDBSettings.Value.DatabaseName);
            return Ok("All data has been cleared.");
        }
        [HttpGet]
        public async Task<IActionResult> GetStudents() => Ok(await _studentService.GetAsync());


        [HttpGet]
        public async Task<IActionResult> GetLecturers() => Ok(await _lecturerService.GetAsync());
    }
    }
