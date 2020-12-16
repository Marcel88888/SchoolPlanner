using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace SchoolPlanner.Controllers {
    public class TeacherController : Controller {
        private readonly ILogger<TeacherController> _logger;
        private readonly SchoolPlannerContext _context;

        public TeacherController(ILogger<TeacherController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index() {
            Helper helper = new Helper();
            var teachers = await _context.Teacher.OrderBy(t => t.Surname).ToListAsync();
            ViewData["teachers"] = teachers; 
            return View(helper);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Helper helper) { 
            ViewData["teachers"] = await _context.Teacher.OrderBy(t => t.Surname).ToListAsync();
            ViewData["all_lessons"] = await _context.Lesson.ToListAsync();
            var chosen_lessons = await _context.Lesson
                       .Where(l => l.Teacher.Id == helper.ChosenTeacher)
                       .Include(l => l.Class)
                       .Include(l => l.Subject)
                       .Include(l => l.Classroom)
                       .Include(l => l.Teacher)
                       .ToListAsync();
            ViewData["chosen_lessons"] = chosen_lessons;
            return View(helper);
        }

        public async Task<IActionResult> AddLesson(Helper helper, int chosenTeacher, int slot) {
            var classesOptions = new List<Class>();
            var classes = await _context.Class.ToListAsync();
            var lessons = await _context.Lesson.ToListAsync();
            foreach (Class _class in classes) {
                bool classAvailable = true;
                foreach (Lesson lesson in lessons) {
                    if (_class.Name == lesson.Class.Name && slot == lesson.Slot) {
                        classAvailable = false;
                        break;
                    }
                }
                if (classAvailable) {
                    classesOptions.Add(_class);
                }
            }
            var classroomsOptions = new List<Classroom>();
            var classrooms = await _context.Classroom.ToListAsync();
            foreach (Classroom classroom in classrooms) {
                bool classroomAvailable = true;
                foreach (Lesson lesson in lessons) {
                    if (classroom.Number == lesson.Classroom.Number && slot == lesson.Slot) {
                        classroomAvailable = false;
                        break;
                    }
                }
                if (classroomAvailable) {
                    classroomsOptions.Add(classroom);
                }
            }

            ViewData["classes_options"] = classesOptions;
            ViewData["classrooms_options"] = classroomsOptions;
            ViewData["chosen_teacher"] = chosenTeacher;
            ViewData["slot"] = slot;
            ViewData["subjects"] = await _context.Subject.ToListAsync();

            return View(helper);
        }


        public async Task<IActionResult> SuccessfulLessonAdding(int chosenTeacher, int slot) {
            var classes = from c in _context.Class
                            where c.Id == (int)TempData["class"]
                            select c;
            var _class = await classes.SingleOrDefaultAsync();
            var subjects = from s in _context.Subject
                            where s.Id == (int)TempData["subject"]
                            select s;
            var subject = await subjects.SingleOrDefaultAsync();
            var classrooms = from c in _context.Classroom
                            where c.Id == (int)TempData["classroom"]
                            select c;
            var classroom = await classrooms.SingleOrDefaultAsync();
            var teachers = from t in _context.Teacher
                            where t.Id == chosenTeacher
                            select t;
            var teacher = await teachers.SingleOrDefaultAsync();

            if (_class == null || subject == null || classroom == null || teacher == null) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The value you tried to select had been deleted by another user before." });
            }

            _context.Add(new Lesson {
                Classroom = classroom,
                Class = _class,
                Subject = subject,
                Slot = slot,
                Teacher = teacher,
            });
            await _context.SaveChangesAsync();
            return View();     
        }

        public IActionResult UnsuccessfulLessonAdding(int chosenTeacher, int slot) {
            ViewData["chosen_teacher"] = chosenTeacher;
            ViewData["slot"] = slot;
            return View();
        }

        public async Task<IActionResult> EditLesson(Helper helper, int id, int slot) {
            var classesOptions = new List<Class>();
            var classes = await _context.Class.ToListAsync();
            var lessons = await _context.Lesson.ToListAsync();
            var chosenLessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var chosenLesson = await chosenLessons.SingleOrDefaultAsync();
            if (chosenLesson == null) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The lesson you tried to modify had been deleted by another user before." });
            }
            classesOptions.Add(chosenLesson.Class);
            foreach (Class _class in classes) {
                bool classAvailable = true;
                foreach (Lesson lesson in lessons) {
                    if (_class.Name == lesson.Class.Name && chosenLesson.Slot == lesson.Slot) {
                        classAvailable = false;
                        break;
                    }
                }
                if (classAvailable) {
                    classesOptions.Add(_class);
                }
            }
            var classroomsOptions = new List<Classroom>();
            var classrooms = await _context.Classroom.ToListAsync();
            classroomsOptions.Add(chosenLesson.Classroom);
            foreach (Classroom classroom in classrooms) {
                bool classroomAvailable = true;
                foreach (Lesson lesson in lessons) {
                    if (classroom.Number == lesson.Classroom.Number && chosenLesson.Slot == lesson.Slot) {
                        classroomAvailable = false;
                        break;
                    }
                }
                if (classroomAvailable) {
                    classroomsOptions.Add(classroom);
                }
            }

            ViewData["lesson_to_edit_id"] = id;
            ViewData["lesson_timestamp"] = chosenLesson.Timestamp;
            ViewData["classes_options"] = classesOptions;
            ViewData["classrooms_options"] = classroomsOptions;
            ViewData["slot"] = slot;
            ViewData["subjects"] = await _context.Subject.ToListAsync();
            ViewData["selected_class"] = chosenLesson.Class.Id;
            ViewData["selected_subject"] = chosenLesson.Subject.Id;
            ViewData["selected_classroom"] = chosenLesson.Classroom.Id; 

            return View(helper);
        }
        public async Task<IActionResult> SuccessfulLessonEdit(int id, DateTime currentTimestamp) {
            var classes = from c in _context.Class
                            where c.Id == (int)TempData["class"]
                            select c;
            var _class = await classes.SingleOrDefaultAsync();
            var subjects = from s in _context.Subject
                            where s.Id == (int)TempData["subject"]
                            select s;
            var subject = await subjects.SingleOrDefaultAsync();
            var classrooms = from c in _context.Classroom
                            where c.Id == (int)TempData["classroom"]
                            select c;
            var classroom = await classrooms.SingleOrDefaultAsync();
            
            var lessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var lesson = await lessons.SingleOrDefaultAsync();

            if (lesson == null) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The lesson you tried to modify had been deleted by another user before." });
            }

            if (_class == null || subject == null || classroom == null) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The value you tried to select had been deleted by another user before." });
            }
            
            try {
                lesson.Class = _class;
                lesson.Subject = subject;
                lesson.Classroom = classroom;
                _context.Entry(lesson).Property("Timestamp").OriginalValue = currentTimestamp;
                _context.Lesson.Update(lesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The lesson you attempted to edit was modified by another user after you got the original value." });
            }

            return View();
        }

        public IActionResult UnsuccessfulLessonEdit(int id) {
            ViewData["id"] = id;
            return View();
        }

        public async Task<IActionResult> DeleteLesson(int id, DateTime currentTimestamp) {
            var lessons = from l in _context.Lesson
                            where l.Id == id
                            select l;
            var lesson = await lessons.SingleOrDefaultAsync();
            if (lesson == null) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The lesson you tried to delete had been deleted by another user before." });
            }
            try {
                _context.Entry(lesson).Property("Timestamp").OriginalValue = currentTimestamp;
                _context.Lesson.Remove(lesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return RedirectToAction("ConcurrencyError", new { errorMessage = "The lesson you attempted to delete was modified by another user after you got the original value." });
            }
            return RedirectToAction("Index");
        }

        public IActionResult ValidateAddedData(Helper helper, int chosenTeacher, int slot) {
            if (helper.NewLesson.Class == null || helper.NewLesson.Subject == null || helper.NewLesson.Classroom == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenTeacher = chosenTeacher, slot = slot });
            }
            else {
                TempData["class"] = helper.NewLesson.Class;
                TempData["subject"] = helper.NewLesson.Subject;
                TempData["classroom"] = helper.NewLesson.Classroom;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenTeacher = chosenTeacher, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Helper helper, int id, DateTime currentTimestamp) {
            if (helper.EditedLesson.Class == null || helper.EditedLesson.Subject == null || helper.EditedLesson.Classroom == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["class"] = helper.EditedLesson.Class;
                TempData["subject"] = helper.EditedLesson.Subject;
                TempData["classroom"] = helper.EditedLesson.Classroom;
                return RedirectToAction("SuccessfulLessonEdit", new { id = id, currentTimestamp = currentTimestamp });
            }
        }

        public IActionResult ConcurrencyError(string errorMessage) {
            ViewData["error_message"] = errorMessage;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}