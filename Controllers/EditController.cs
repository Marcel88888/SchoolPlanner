using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using System.Linq;
using System;


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
        public IActionResult Index(Edit edit) {
            if (edit.ClassroomToAdd != null) {
                try {
                    _context.Classroom.Add(new Classroom {
                            Number = edit.ClassroomToAdd,
                            Timestamp = DateTime.Now
                            });
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.ClassroomToDelete != null) {
                var _classrooms = from c in _context.Classroom
                                where c.Id == edit.ClassroomToDelete
                                select c;
                var classroom = _classrooms.Single();
                var associatedLessons = from l in _context.Lesson
                                        where l.Classroom.Id == classroom.Id
                                        select l;
                if (associatedLessons.ToList().Count() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Classroom.Remove(classroom);
                _context.SaveChanges();

            }
            else if (edit.ClassToAdd != null) {
                try {
                    _context.Class.Add(new Class {
                            Name = edit.ClassToAdd,
                            Timestamp = DateTime.Now
                            });
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.ClassToDelete != null) {
                var _classes = from c in _context.Class
                                where c.Id == edit.ClassToDelete
                                select c;
                var _class = _classes.Single();
                var associatedLessons = from l in _context.Lesson
                                        where l.Class.Id == _class.Id
                                        select l;
                if (associatedLessons.ToList().Count() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Class.Remove(_class);
                _context.SaveChanges();
                
            }
            else if (edit.SubjectToAdd != null) {
                try {
                    _context.Subject.Add(new Subject {
                            Name = edit.SubjectToAdd,
                            Timestamp = DateTime.Now
                            });
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.SubjectToDelete != null) {
                var _subjects = from s in _context.Subject
                                where s.Id == edit.SubjectToDelete
                                select s;
                var subject = _subjects.Single();
                var associatedLessons = from l in _context.Lesson
                                        where l.Subject.Id == subject.Id
                                        select l;
                if (associatedLessons.ToList().Count() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Subject.Remove(subject);
                _context.SaveChanges();
            }
            else if (edit.TeacherToAdd != null) {
                try {
                    _context.Teacher.Add(new Teacher {
                            Surname = edit.TeacherToAdd,
                            Timestamp = DateTime.Now
                            });
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.TeacherToDelete != null) {
                var _teachers = from t in _context.Teacher
                                where t.Id == edit.TeacherToDelete
                                select t;
                var teacher = _teachers.Single();
                var associatedLessons = from l in _context.Lesson
                                        where l.Teacher.Id == teacher.Id
                                        select l;
                if (associatedLessons.ToList().Count() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
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

        public IActionResult UnsuccessfulAdding(Reader reader, int id) {
            return View();
        }

        public IActionResult UnsuccessfulDeleting(Reader reader, int id) {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
