using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace SchoolPlanner.Controllers {
    public class EditController : Controller {
        private readonly ILogger<HomeController> _logger;
        private SchoolPlannerContext _context;

        public EditController(ILogger<HomeController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index() {
            Edit edit = new Edit();
            var classes = await _context.Class.OrderBy(c => c.Name).ToListAsync();
            var classrooms = await _context.Classroom.OrderBy(c => c.Number).ToListAsync();
            var teachers = await _context.Teacher.OrderBy(t => t.Surname).ToListAsync();
            var subjects = await _context.Subject.OrderBy(s => s.Name).ToListAsync();
            ViewData["classes"] = classes; 
            ViewData["classrooms"] = classrooms; 
            ViewData["teachers"] = teachers; 
            ViewData["subjects"] = subjects; 
            return View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Edit edit) {
            if (edit.ClassroomToAdd != null) {
                try {
                    _context.Classroom.Add(new Classroom {
                            Number = edit.ClassroomToAdd,
                            });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.ClassroomToDelete != null) {
                var _classrooms = from c in _context.Classroom
                                where c.Id == edit.ClassroomToDelete
                                select c;
                var classroom = await _classrooms.SingleOrDefaultAsync();
                if (classroom == null) {
                    return RedirectToAction("ConcurrencyUnsuccessfulEdition");
                }
                var associatedLessons = from l in _context.Lesson
                                        where l.Classroom.Id == classroom.Id
                                        select l;
                if (await associatedLessons.CountAsync() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Classroom.Remove(classroom);
                await _context.SaveChangesAsync();

            }
            else if (edit.ClassToAdd != null) {
                try {
                    _context.Class.Add(new Class {
                            Name = edit.ClassToAdd,
                            });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.ClassToDelete != null) {
                var _classes = from c in _context.Class
                                where c.Id == edit.ClassToDelete
                                select c;
                var _class = await _classes.SingleOrDefaultAsync();
                if (_class == null) {
                    return RedirectToAction("ConcurrencyUnsuccessfulEdition");
                }
                var associatedLessons = from l in _context.Lesson
                                        where l.Class.Id == _class.Id
                                        select l;
                if (await associatedLessons.CountAsync() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Class.Remove(_class);
                await _context.SaveChangesAsync();
                
            }
            else if (edit.SubjectToAdd != null) {
                try {
                    _context.Subject.Add(new Subject {
                            Name = edit.SubjectToAdd,
                            // Timestamp = DateTime.Now
                            });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.SubjectToDelete != null) {
                var _subjects = from s in _context.Subject
                                where s.Id == edit.SubjectToDelete
                                select s;
                var subject = await _subjects.SingleOrDefaultAsync();
                if (subject == null) {
                    return RedirectToAction("ConcurrencyUnsuccessfulEdition");
                }
                var associatedLessons = from l in _context.Lesson
                                        where l.Subject.Id == subject.Id
                                        select l;
                if (await associatedLessons.CountAsync() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Subject.Remove(subject);
                await _context.SaveChangesAsync();
            }
            else if (edit.TeacherToAdd != null) {
                try {
                    _context.Teacher.Add(new Teacher {
                            Surname = edit.TeacherToAdd,
                            // Timestamp = DateTime.Now
                            });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException) {
                    return RedirectToAction("UnsuccessfulAdding");
                }
            }
            else if (edit.TeacherToDelete != null) {
                var _teachers = from t in _context.Teacher
                                where t.Id == edit.TeacherToDelete
                                select t;
                var teacher = await _teachers.SingleOrDefaultAsync();
                if (teacher == null) {
                    return RedirectToAction("ConcurrencyUnsuccessfulEdition");
                }
                var associatedLessons = from l in _context.Lesson
                                        where l.Teacher.Id == teacher.Id
                                        select l;
                if (await associatedLessons.CountAsync() > 0) {
                    return RedirectToAction("UnsuccessfulDeleting");
                }
                _context.Teacher.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            var classes = await _context.Class.OrderBy(c => c.Name).ToListAsync();
            var classrooms = await _context.Classroom.OrderBy(c => c.Number).ToListAsync();
            var teachers = await _context.Teacher.OrderBy(t => t.Surname).ToListAsync();
            var subjects = await _context.Subject.OrderBy(s => s.Name).ToListAsync();
            ViewData["classes"] = classes; 
            ViewData["classrooms"] = classrooms; 
            ViewData["teachers"] = teachers; 
            ViewData["subjects"] = subjects;
            return View(edit);
        }

        public IActionResult UnsuccessfulAdding() {
            return View();
        }

        public IActionResult UnsuccessfulDeleting() {
            return View();
        }

        public IActionResult ConcurrencyUnsuccessfulEdition() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
