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
                try {
                    var _classrooms = from c in _context.Classroom
                                    where c.Number == edit.ClassroomToDelete
                                    select c;
                    var classroom = _classrooms.Single();
                    _context.Classroom.Remove(classroom);
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
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
                try {
                    var _classes = from c in _context.Class
                                    where c.Name == edit.ClassToDelete
                                    select c;
                    var _class = _classes.Single();
                    _context.Class.Remove(_class);
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
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
                try {
                    var _subjects = from s in _context.Subject
                                    where s.Name == edit.SubjectToDelete
                                    select s;
                    var subject = _subjects.Single();
                    _context.Subject.Remove(subject);
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
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
                try {
                    var _teachers = from t in _context.Teacher
                                    where t.Surname == edit.TeacherToDelete
                                    select t;
                    var teacher = _teachers.Single();
                    _context.Teacher.Remove(teacher);
                    _context.SaveChanges();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
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
