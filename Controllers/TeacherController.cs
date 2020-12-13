using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using Microsoft.EntityFrameworkCore;


namespace SchoolPlanner.Controllers {
    public class TeacherController : Controller {
        private readonly ILogger<TeacherController> _logger;
        private readonly SchoolPlannerContext _context;

        public TeacherController(ILogger<TeacherController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            Reader reader = new Reader();
            var teachers = _context.Teacher.OrderBy(t => t.Surname).ToList();
            ViewData["teachers"] = teachers; 
            return View(reader);
        }

        [HttpPost]
        public IActionResult Index(Reader reader) { 
            ViewData["teachers"] = _context.Teacher.OrderBy(t => t.Surname).ToList();
            ViewData["all_lessons"] = _context.Lesson.ToList();
            var chosen_lessons = _context.Lesson
                       .Where(l => l.Teacher.Surname == reader.ChosenTeacher)
                       .Include(l => l.Class)
                       .Include(l => l.Subject)
                       .Include(l => l.Classroom)
                       .Include(l => l.Teacher)
                       .ToList();
            ViewData["chosen_lessons"] = chosen_lessons.ToList();
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader, string chosenTeacher, int slot) {
            var classesOptions = new List<Class>();
            var classes = _context.Class.ToList();
            var lessons = _context.Lesson.ToList();
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
            var classrooms = _context.Classroom.ToList();
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
            ViewData["subjects"] = _context.Subject.ToList();

            return View(reader);
        }


        public IActionResult SuccessfulLessonAdding(Reader reader, string chosenTeacher, int slot) {
            var classes = from c in _context.Class
                            where c.Name == (string)TempData["class"]
                            select c;
            var _class = classes.Single();
            var subjects = from s in _context.Subject
                            where s.Name == (string)TempData["subject"]
                            select s;
            var subject = subjects.Single();
            var classrooms = from c in _context.Classroom
                            where c.Number == (string)TempData["classroom"]
                            select c;
            var classroom = classrooms.Single();
            var teachers = from t in _context.Teacher
                            where t.Surname == chosenTeacher
                            select t;
            var teacher = teachers.Single();

            _context.Add(new Lesson {
                Classroom = classroom,
                Class = _class,
                Subject = subject,
                Slot = slot,
                Teacher = teacher
            });
            _context.SaveChanges();
            return View();     
        }

        public IActionResult UnsuccessfulLessonAdding(Reader reader, string chosenTeacher, int slot) {
            ViewData["chosen_teacher"] = chosenTeacher;
            ViewData["slot"] = slot;
            return View();
        }

        public IActionResult EditLesson(Reader reader, int id, int slot) {
            var classesOptions = new List<Class>();
            var classes = _context.Class.ToList();
            var lessons = _context.Lesson.ToList();
            var chosenLessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var chosenLesson = chosenLessons.Single();
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
            var classrooms = _context.Classroom.ToList();
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
            ViewData["classes_options"] = classesOptions;
            ViewData["classrooms_options"] = classroomsOptions;
            ViewData["slot"] = slot;
            ViewData["subjects"] = _context.Subject.ToList();
            ViewData["selected_class"] = chosenLesson.Class.Name;
            ViewData["selected_subject"] = chosenLesson.Subject.Name;
            ViewData["selected_classroom"] = chosenLesson.Classroom.Number; 

            return View(reader);
        }
        public IActionResult SuccessfulLessonEdit(Reader reader, int id) {
            var classes = from c in _context.Class
                            where c.Name == (string)TempData["class"]
                            select c;
            var _class = classes.Single();
            var subjects = from s in _context.Subject
                            where s.Name == (string)TempData["subject"]
                            select s;
            var subject = subjects.Single();
            var classrooms = from c in _context.Classroom
                            where c.Number == (string)TempData["classroom"]
                            select c;
            var classroom = classrooms.Single();
            
            var lessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var lesson = lessons.Single();
            
            lesson.Class = _class;
            lesson.Subject = subject;
            lesson.Classroom = classroom;
            _context.SaveChanges();

            return View();
        }

        public IActionResult UnsuccessfulLessonEdit(Reader reader, int id) {
            ViewData["id"] = id;
            return View();
        }

        public IActionResult DeleteLesson(Reader reader, int id) {
            var lessons = from l in _context.Lesson
                            where l.Id == id
                            select l;
            var lesson = lessons.Single();
            _context.Lesson.Remove(lesson);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ValidateAddedData(Reader reader, string chosenTeacher, int slot) {
            if (reader.NewLesson.Class == null || reader.NewLesson.Subject == null || reader.NewLesson.Classroom == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenTeacher = chosenTeacher, slot = slot });
            }
            else {
                TempData["class"] = reader.NewLesson.Class;
                TempData["subject"] = reader.NewLesson.Subject;
                TempData["classroom"] = reader.NewLesson.Classroom;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenTeacher = chosenTeacher, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Reader reader, int id) {
            if (reader.EditedLesson.Class == null || reader.EditedLesson.Subject == null || reader.EditedLesson.Classroom == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["class"] = reader.EditedLesson.Class;
                TempData["subject"] = reader.EditedLesson.Subject;
                TempData["classroom"] = reader.EditedLesson.Classroom;
                return RedirectToAction("SuccessfulLessonEdit", new { id = id });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}