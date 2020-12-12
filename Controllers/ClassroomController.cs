using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolPlanner.Controllers {
    public class ClassroomController : Controller {
        private readonly ILogger<ClassroomController> _logger;
        private readonly SchoolPlannerContext _context;

        public ClassroomController(ILogger<ClassroomController> logger, SchoolPlannerContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            Reader reader = new Reader();
            var classrooms = _context.Classroom.OrderBy(c => c.Number).ToList();
            ViewData["classrooms"] = classrooms; 
            return View(reader);
        }

        [HttpPost]
        public IActionResult Index(Reader reader) { 
            ViewData["classrooms"] = _context.Classroom.OrderBy(c => c.Number).ToList();
            ViewData["all_lessons"] = _context.Lesson.ToList();
            var chosen_lessons = _context.Lesson
                       .Where(l => l.Classroom.Number == reader.ChosenClassroom)
                       .Include(l => l.Class)
                       .Include(l => l.Subject)
                       .Include(l => l.Classroom)
                       .Include(l => l.Teacher)
                       .ToList();
            ViewData["chosen_lessons"] = chosen_lessons.ToList();
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader, string chosenClassroom, int slot) {
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

            ViewData["classes_options"] = classesOptions;
            ViewData["teachers_options"] = teachersOptions;
            ViewData["chosen_classroom"] = chosenClassroom;
            ViewData["slot"] = slot;
            ViewData["subjects"] = _context.Subject.ToList();

            return View(reader);
        }

        public IActionResult SuccessfulLessonAdding(Reader reader, string chosenClassroom, int slot) {
            // reader.NewLesson = new Lesson();
            // Class _class = (Class)TempData["class"];
            // Subject subject = (Subject)TempData["subject"];
            // Teacher teacher = (Teacher)TempData["teacher"];
            // reader.NewLesson.Classroom = chosenClassroom;
            // reader.NewLesson.Class = _class;
            // reader.NewLesson.Subject = subject;
            // reader.NewLesson.Slot = slot;
            // reader.NewLesson.Teacher = teacher;
            // reader.Lessons.Add(reader.NewLesson);
            // reader.updateJsonFile();

            // TempData["class"] = reader.NewLesson.Class;
            //     TempData["subject"] = reader.NewLesson.Subject;
            //     TempData["teacher"] = reader.NewLesson.Teacher;
            //     TempData["classroom"] = chosenClassroom;
            //     TempData["slot"] = slot;


            var classes = from c in _context.Class
                            where c.Name == (string)TempData["class"]
                            select c;
            var _class = classes.Single();
            var subjects = from s in _context.Subject
                            where s.Name == (string)TempData["subject"]
                            select s;
            var subject = subjects.Single();
            var teachers = from t in _context.Teacher
                            where t.Surname == (string)TempData["teacher"]
                            select t;
            var teacher = teachers.Single();
            var classrooms = from c in _context.Classroom
                            where c.Number == (string)TempData["classroom"]
                            select c;
            var classroom = classrooms.Single();

            _context.Add(new Lesson {
                Classroom = classroom,
                Class = _class,
                Subject = subject,
                Slot = (int)TempData["slot"],
                Teacher = teacher
            });
            _context.SaveChanges();
            return View();     
        }

        public IActionResult UnsuccessfulLessonAdding(Reader reader, string chosenClassroom, int slot) {
            ViewData["chosen_classroom"] = chosenClassroom;
            ViewData["slot"] = slot;
            return View();
        }

        public IActionResult EditLesson(Reader reader, int id) {
            // reader.ClassesOptions = new List<Class>();
            // Class selectedClass = new Class(reader.Lessons[id].Class);
            // reader.ClassesOptions.Add(selectedClass);
            // foreach (Class _class in reader.Classes) {
            //     bool classAvailable = true;
            //     foreach (Lesson lesson in reader.Lessons) {
            //         if (_class.Name == lesson.Class && reader.Lessons[id].Slot == lesson.Slot) {
            //             classAvailable = false;
            //             break;
            //         }
            //     }
            //     if (classAvailable) {
            //         reader.ClassesOptions.Add(_class);
            //     }
            // }
            // reader.TeachersOptions = new List<Teacher>();
            // Teacher selectedTeacher = new Teacher(reader.Lessons[id].Teacher);
            // reader.TeachersOptions.Add(selectedTeacher);
            // foreach (Teacher teacher in reader.Teachers) {
            //     bool teacherAvailable = true;
            //     foreach (Lesson lesson in reader.Lessons) {
            //         if (teacher.Surname == lesson.Teacher && reader.Lessons[id].Slot == lesson.Slot) {
            //             teacherAvailable = false;
            //         }
            //     }
            //     if (teacherAvailable) {
            //         reader.TeachersOptions.Add(teacher);
            //     }
            // }
            // ViewData["lessonToEditIndex"] = id;
            return View(reader);
        }

        public IActionResult SuccessfulLessonEdit(Reader reader, int id) {
            string _class = (string)TempData["class"];
            string subject = (string)TempData["subject"];
            string teacher = (string)TempData["teacher"];
            // reader.Lessons[id].Class = _class;
            // reader.Lessons[id].Subject = subject;
            // reader.Lessons[id].Teacher = teacher;
            reader.updateJsonFile();
            return View();
        }

        public IActionResult UnsuccessfulLessonEdit(Reader reader, int id) {
            ViewData["id"] = id;
            return View();
        }

        public IActionResult DeleteLesson(Reader reader, int id) {
            reader.Lessons.RemoveAt(id);
            reader.updateJsonFile();
            return RedirectToAction("Index");
        }

        public IActionResult ValidateAddedData(Reader reader, string chosenClassroom, int slot) {
            if (reader.NewLesson.Class == null || reader.NewLesson.Subject == null || reader.NewLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenClassroom = chosenClassroom, slot = slot });
            }
            else {
                TempData["class"] = reader.NewLesson.Class;
                TempData["subject"] = reader.NewLesson.Subject;
                TempData["teacher"] = reader.NewLesson.Teacher;
                TempData["classroom"] = chosenClassroom;
                TempData["slot"] = slot;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenClassroom = chosenClassroom, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Reader reader, Lesson editedLesson, int id) {
            if (editedLesson.Class == null || editedLesson.Subject == null || editedLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["class"] = editedLesson.Class;
                TempData["subject"] = editedLesson.Subject;
                TempData["teacher"] = editedLesson.Teacher;
                return RedirectToAction("SuccessfulLessonEdit", new { id = id });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}