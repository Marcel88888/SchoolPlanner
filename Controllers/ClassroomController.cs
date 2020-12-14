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
    public class ClassroomController : Controller {
        private readonly ILogger<ClassroomController> _logger;
        private readonly SchoolPlannerContext _context;

        public ClassroomController(ILogger<ClassroomController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index() {
            Reader reader = new Reader();
            var classrooms = await _context.Classroom.OrderBy(c => c.Number).ToListAsync();
            ViewData["classrooms"] = classrooms; 
            return View(reader);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Reader reader) { 
            ViewData["classrooms"] = await _context.Classroom.OrderBy(c => c.Number).ToListAsync();
            ViewData["all_lessons"] = await _context.Lesson.ToListAsync();
            var chosen_lessons = await _context.Lesson
                       .Where(l => l.Classroom.Id == reader.ChosenClassroom)
                       .Include(l => l.Class)
                       .Include(l => l.Subject)
                       .Include(l => l.Classroom)
                       .Include(l => l.Teacher)
                       .ToListAsync();
            ViewData["chosen_lessons"] = chosen_lessons;
            return View(reader);
        }

        public async Task<IActionResult> AddLesson(Reader reader, int chosenClassroom, int slot) {
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
            var teachersOptions = new List<Teacher>();
            var teachers = await _context.Teacher.ToListAsync();
            foreach (Teacher teacher in teachers) {
                bool teacherAvailable = true;
                foreach (Lesson lesson in lessons) {
                    if (teacher.Surname == lesson.Teacher.Surname && slot == lesson.Slot) {
                        teacherAvailable = false;
                        break;
                    }
                }
                if (teacherAvailable) {
                    teachersOptions.Add(teacher);
                }
            }

            ViewData["classes_options"] = classesOptions;
            ViewData["teachers_options"] = teachersOptions;
            ViewData["chosen_classroom"] = chosenClassroom;
            ViewData["slot"] = slot;
            ViewData["subjects"] = await _context.Subject.ToListAsync();

            return View(reader);
        }

        public async Task<IActionResult> SuccessfulLessonAdding(Reader reader, int chosenClassroom, int slot) {
            var classes = from c in _context.Class
                            where c.Id == (int)TempData["class"]
                            select c;
            var _class = await classes.SingleOrDefaultAsync();
            var subjects = from s in _context.Subject
                            where s.Id == (int)TempData["subject"]
                            select s;
            var subject = await subjects.SingleOrDefaultAsync();
            var teachers = from t in _context.Teacher
                            where t.Id == (int)TempData["teacher"]
                            select t;
            var teacher = await teachers.SingleOrDefaultAsync();
            var classrooms = from c in _context.Classroom
                            where c.Id == chosenClassroom
                            select c;
            var classroom = await classrooms.SingleOrDefaultAsync();

            if (_class == null || subject == null || teacher == null || classroom == null) {
                return RedirectToAction("ConcurrencyUnsuccessfulEdition");
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

        public IActionResult UnsuccessfulLessonAdding(Reader reader, int chosenClassroom, int slot) {
            ViewData["chosen_classroom"] = chosenClassroom;
            ViewData["slot"] = slot;
            return View();
        }

        public async Task<IActionResult> EditLesson(Reader reader, int id, int slot) {
            var classesOptions = new List<Class>();
            var classes = await _context.Class.ToListAsync();
            var lessons = await _context.Lesson.ToListAsync();
            var chosenLessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var chosenLesson = await chosenLessons.SingleOrDefaultAsync();
            if (chosenLesson == null) {
                return RedirectToAction("ConcurrencyUnsuccessfulEdition");
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
            var teachersOptions = new List<Teacher>();
            var teachers = await _context.Teacher.ToListAsync();
            teachersOptions.Add(chosenLesson.Teacher);
            foreach (Teacher teacher in teachers) {
                bool teacherAvailable = true;
                foreach (Lesson lesson in lessons) {
                    if (teacher.Surname == lesson.Teacher.Surname && chosenLesson.Slot == lesson.Slot) {
                        teacherAvailable = false;
                        break;
                    }
                }
                if (teacherAvailable) {
                    teachersOptions.Add(teacher);
                }
            }

            ViewData["lesson_to_edit_id"] = id;
            ViewData["lesson_timestamp"] = chosenLesson.Timestamp;
            ViewData["classes_options"] = classesOptions;
            ViewData["teachers_options"] = teachersOptions;
            ViewData["slot"] = slot;
            ViewData["subjects"] = await _context.Subject.ToListAsync();
            ViewData["selected_class"] = chosenLesson.Class.Id;
            ViewData["selected_subject"] = chosenLesson.Subject.Id;
            ViewData["selected_teacher"] = chosenLesson.Teacher.Id; 

            return View(reader);
        }

        public async Task<IActionResult> SuccessfulLessonEdit(Reader reader, int id, DateTime currentTimestamp) {
            var classes = from c in _context.Class
                            where c.Id == (int)TempData["class"]
                            select c;
            var _class = await classes.SingleOrDefaultAsync();
            var subjects = from s in _context.Subject
                            where s.Id == (int)TempData["subject"]
                            select s;
            var subject = await subjects.SingleOrDefaultAsync();
            var teachers = from t in _context.Teacher
                            where t.Id == (int)TempData["teacher"]
                            select t;
            var teacher = await teachers.SingleOrDefaultAsync();
            
            var lessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var lesson = await lessons.SingleOrDefaultAsync();

            if (_class == null || subject == null || teacher == null || lesson == null) {
                return RedirectToAction("ConcurrencyUnsuccessfulEdition");
            }

            try {
                lesson.Class = _class;
                lesson.Subject = subject;
                lesson.Teacher = teacher;
                _context.Entry(lesson).Property("Timestamp").OriginalValue = currentTimestamp;
                _context.Lesson.Update(lesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return RedirectToAction("ConcurrencyDataEdition");
            }

            return View();
        }

        public IActionResult UnsuccessfulLessonEdit(Reader reader, int id) {
            ViewData["id"] = id;
            return View();
        }

        public async Task<IActionResult> DeleteLesson(Reader reader, int id, DateTime currentTimestamp) {
            var lessons = from l in _context.Lesson
                            where l.Id == id
                            select l;
            var lesson = await lessons.SingleOrDefaultAsync();
            if (lesson == null) {
                return RedirectToAction("ConcurrencyUnsuccessfulEdition");
            }
            try {
                _context.Entry(lesson).Property("Timestamp").OriginalValue = currentTimestamp;
                _context.Lesson.Remove(lesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return RedirectToAction("ConcurrencyDataEdition");
            }
            return RedirectToAction("Index");
        }

        public IActionResult ValidateAddedData(Reader reader, int chosenClassroom, int slot) {
            if (reader.NewLesson.Class == null || reader.NewLesson.Subject == null || reader.NewLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenClassroom = chosenClassroom, slot = slot });
            }
            else {
                TempData["class"] = reader.NewLesson.Class;
                TempData["subject"] = reader.NewLesson.Subject;
                TempData["teacher"] = reader.NewLesson.Teacher;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenClassroom = chosenClassroom, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Reader reader, int id, DateTime currentTimestamp) {
            if (reader.EditedLesson.Class == null || reader.EditedLesson.Subject == null || reader.EditedLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["class"] = reader.EditedLesson.Class;
                TempData["subject"] = reader.EditedLesson.Subject;
                TempData["teacher"] = reader.EditedLesson.Teacher;
                return RedirectToAction("SuccessfulLessonEdit", new { id = id, currentTimestamp = currentTimestamp });
            }
        }

        public IActionResult ConcurrencyUnsuccessfulEdition() {
            return View();
        }
        public IActionResult ConcurrencyDataEdition() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}