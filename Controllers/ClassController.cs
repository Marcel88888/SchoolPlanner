using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SchoolPlanner.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace SchoolPlanner.Controllers {
    public class ClassController : Controller {
        private readonly ILogger<ClassController> _logger;
        private readonly SchoolPlannerContext _context;

        public ClassController(ILogger<ClassController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            Reader reader = new Reader();
            var classes = _context.Class.OrderBy(c => c.Name).ToList();
            ViewData["classes"] = classes; 
            return View(reader);
        }

        [HttpPost]
        public IActionResult Index(Reader reader) {     // TODO: add "string _class" as a parameter 
            ViewData["classes"] = _context.Class.OrderBy(c => c.Name).ToList();
            ViewData["all_lessons"] = _context.Lesson.ToList();
            var chosen_lessons = _context.Lesson
                       .Where(l => l.Class.Name == reader.ChosenClass)
                       .Include(l => l.Class)
                       .Include(l => l.Subject)
                       .Include(l => l.Classroom)
                       .Include(l => l.Teacher)
                       .ToList();
            ViewData["chosen_lessons"] = chosen_lessons.ToList();
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader, string chosenClass, int slot) {
            var classroomsOptions = new List<Classroom>();
            var classrooms = _context.Classroom.ToList();
            var lessons = _context.Lesson.ToList();
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
            var teachersOptions = new List<Teacher>();
            var teachers = _context.Teacher.ToList();
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

            ViewData["classrooms_options"] = classroomsOptions;
            ViewData["teachers_options"] = teachersOptions;
            ViewData["chosen_class"] = chosenClass;
            ViewData["slot"] = slot;
            ViewData["subjects"] = _context.Subject.ToList();

            return View(reader);
        }

        public IActionResult SuccessfulLessonAdding(Reader reader, string chosenClass, int slot) {
            var classrooms = from c in _context.Classroom
                            where c.Number == (string)TempData["classroom"]
                            select c;
            var classroom = classrooms.Single();
            var subjects = from s in _context.Subject
                            where s.Name == (string)TempData["subject"]
                            select s;
            var subject = subjects.Single();
            var teachers = from t in _context.Teacher
                            where t.Surname == (string)TempData["teacher"]
                            select t;
            var teacher = teachers.Single();
            var classes = from c in _context.Class
                            where c.Name == chosenClass
                            select c;
            var _class = classes.Single();

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

        public IActionResult UnsuccessfulLessonAdding(Reader reader, string chosenClass, int slot) {
            ViewData["chosen_class"] = chosenClass;
            ViewData["slot"] = slot;
            return View();
        }

        public IActionResult EditLesson(Reader reader, int id, int slot) {
            var classroomsOptions = new List<Classroom>();
            var classrooms = _context.Classroom.ToList();
            var lessons = _context.Lesson.ToList();
            var chosenLessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var chosenLesson = chosenLessons.Single();
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
            var teachersOptions = new List<Teacher>();
            var teachers = _context.Teacher.ToList();
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
            ViewData["classrooms_options"] = classroomsOptions;
            ViewData["teachers_options"] = teachersOptions;
            ViewData["slot"] = slot;
            ViewData["subjects"] = _context.Subject.ToList();
            ViewData["selected_classroom"] = chosenLesson.Classroom.Number;
            ViewData["selected_subject"] = chosenLesson.Subject.Name;
            ViewData["selected_teacher"] = chosenLesson.Teacher.Surname; 

            return View(reader);
        }

        public IActionResult SuccessfulLessonEdit(Reader reader, int id) {
            var classrooms = from c in _context.Classroom
                            where c.Number == (string)TempData["classroom"]
                            select c;
            var classroom = classrooms.Single();
            var subjects = from s in _context.Subject
                            where s.Name == (string)TempData["subject"]
                            select s;
            var subject = subjects.Single();
            var teachers = from t in _context.Teacher
                            where t.Surname == (string)TempData["teacher"]
                            select t;
            var teacher = teachers.Single();
            
            var lessons = from l in _context.Lesson
                        where l.Id == id
                        select l;
            var lesson = lessons.Single();
            
            lesson.Classroom = classroom;
            lesson.Subject = subject;
            lesson.Teacher = teacher;
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

        public IActionResult ValidateAddedData(Reader reader, string chosenClass, int slot) {
            if (reader.NewLesson.Classroom == null || reader.NewLesson.Subject == null || reader.NewLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenClass = chosenClass, slot = slot });
            }
            else {
                TempData["classroom"] = reader.NewLesson.Classroom;
                TempData["subject"] = reader.NewLesson.Subject;
                TempData["teacher"] = reader.NewLesson.Teacher;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenClass = chosenClass, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Reader reader, int id) {
            if (reader.EditedLesson.Classroom == null || reader.EditedLesson.Subject == null || reader.EditedLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["classroom"] = reader.EditedLesson.Classroom;
                TempData["subject"] = reader.EditedLesson.Subject;
                TempData["teacher"] = reader.EditedLesson.Teacher;
                return RedirectToAction("SuccessfulLessonEdit", new { id = id });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}