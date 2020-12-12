using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using System.Linq;


namespace SchoolPlanner.Controllers {
    public class EditController : Controller {
        private readonly ILogger<HomeController> _logger;
        private SchoolPlannerContext _context;

        public EditController(ILogger<HomeController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            Edit edit = new Edit();
            Reader reader = new Reader();
            edit._Reader = reader;
            var classes = _context.Class.OrderBy(c => c.Name).ToList();
            var classrooms = _context.Classroom.OrderBy(c => c.Number).ToList();
            var teachers = _context.Teacher.OrderBy(t => t.Surname).ToList();
            var subjects = _context.Subject.OrderBy(s => s.Name).ToList();
            ViewData["classes"] = classes; 
            ViewData["classrooms"] = classrooms; 
            ViewData["teachers"] = teachers; 
            ViewData["subjects"] = subjects; 
            return View(edit);
        }

        [HttpPost]
        public IActionResult Index(Edit edit, Reader reader) {
            if (edit.ClassroomToAdd != null) {
                _context.Classroom.Add(new Classroom {
                        Number = edit.ClassroomToAdd
                        });
                _context.SaveChanges();
            }
            else if (edit.ClassroomToDelete != null) {
                var _classrooms = from c in _context.Classroom
                                where c.Number == edit.ClassroomToDelete
                                select c;
                var classroom = _classrooms.Single();
                _context.Classroom.Remove(classroom);
                _context.SaveChanges();
            }
            else if (edit.ClassToAdd != null) {
                _context.Class.Add(new Class {
                        Name = edit.ClassToAdd
                        });
                _context.SaveChanges();
            }
            else if (edit.ClassToDelete != null) {
                var _classes = from c in _context.Class
                                where c.Name == edit.ClassToDelete
                                select c;
                var _class = _classes.Single();
                _context.Class.Remove(_class);
                _context.SaveChanges();
            }
            else if (edit.SubjectToAdd != null) {
                _context.Subject.Add(new Subject {
                        Name = edit.SubjectToAdd
                        });
                _context.SaveChanges();
            }
            else if (edit.SubjectToDelete != null) {
                var _subjects = from s in _context.Subject
                                where s.Name == edit.SubjectToDelete
                                select s;
                var subject = _subjects.Single();
                _context.Subject.Remove(subject);
                _context.SaveChanges();
            }
            else if (edit.TeacherToAdd != null) {
                _context.Teacher.Add(new Teacher {
                        Surname = edit.TeacherToAdd
                        });
                _context.SaveChanges();
            }
            else if (edit.TeacherToDelete != null) {
                var _teachers = from t in _context.Teacher
                                where t.Surname == edit.TeacherToDelete
                                select t;
                var teacher = _teachers.Single();
                _context.Teacher.Remove(teacher);
                _context.SaveChanges();
            }
            var classes = _context.Class.OrderBy(c => c.Name).ToList();
            var classrooms = _context.Classroom.OrderBy(c => c.Number).ToList();
            var teachers = _context.Teacher.OrderBy(t => t.Surname).ToList();
            var subjects = _context.Subject.OrderBy(s => s.Name).ToList();
            ViewData["classes"] = classes; 
            ViewData["classrooms"] = classrooms; 
            ViewData["teachers"] = teachers; 
            ViewData["subjects"] = subjects;
            return View(edit);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
