using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;

namespace SchoolPlanner.Controllers {
    public class ClassroomController : Controller {
        private readonly ILogger<ClassroomController> _logger;

        public ClassroomController(ILogger<ClassroomController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            Reader reader = new Reader();
            return View(reader);
        }

        [HttpPost]
        public IActionResult Index(Reader reader) {     
            ViewData["all_lessons"] = reader.Lessons;
            ViewData["chosen_lessons"] = reader.getLessonsByClassroom(reader.ChosenClassroom);
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader, string chosenClassroom, int slot) {
            reader.ClassesOptions = new List<Class>();
            foreach (Class _class in reader.Classes) {
                bool classAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (_class.Name == lesson.Class && slot == lesson.Slot) {
                        classAvailable = false;
                        break;
                    }
                }
                if (classAvailable) {
                    reader.ClassesOptions.Add(_class);
                }
            }
            reader.TeachersOptions = new List<Teacher>();
            foreach (Teacher teacher in reader.Teachers) {
                bool teacherAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (teacher.Surname == lesson.Teacher && slot == lesson.Slot) {
                        teacherAvailable = false;
                        break;
                    }
                }
                if (teacherAvailable) {
                    reader.TeachersOptions.Add(teacher);
                }
            }
            ViewData["chosen_classroom"] = chosenClassroom;
            ViewData["slot"] = slot;
            return View(reader);
        }

        public IActionResult SuccessfulLessonAdding(Reader reader, string chosenClassroom, int slot) {
            reader.NewLesson = new Lesson();
            string _class = (string)TempData["class"];
            string subject = (string)TempData["subject"];
            string teacher = (string)TempData["teacher"];
            reader.NewLesson.Classroom = chosenClassroom;
            reader.NewLesson.Class = _class;
            reader.NewLesson.Subject = subject;
            reader.NewLesson.Slot = slot;
            reader.NewLesson.Teacher = teacher;
            reader.Lessons.Add(reader.NewLesson);
            reader.updateJsonFile();
            return View();     
        }

        public IActionResult UnsuccessfulLessonAdding(Reader reader, string chosenClassroom, int slot) {
            ViewData["chosen_classroom"] = chosenClassroom;
            ViewData["slot"] = slot;
            return View();
        }

        public IActionResult EditLesson(Reader reader, int id) {
            reader.ClassesOptions = new List<Class>();
            foreach (Class _class in reader.Classes) {
                bool classAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (_class.Name == lesson.Class && reader.Lessons[id].Slot == lesson.Slot) {
                        classAvailable = false;
                        break;
                    }
                }
                if (classAvailable) {
                    reader.ClassesOptions.Add(_class);
                }
            }
            reader.TeachersOptions = new List<Teacher>();
            foreach (Teacher teacher in reader.Teachers) {
                bool teacherAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (teacher.Surname == lesson.Teacher && reader.Lessons[id].Slot == lesson.Slot) {
                        teacherAvailable = false;
                    }
                }
                if (teacherAvailable) {
                    reader.TeachersOptions.Add(teacher);
                }
            }
            ViewData["lessonToEditIndex"] = id;
            return View(reader);
        }

        public IActionResult SuccessfulLessonEdit(Reader reader, int id) {
            string _class = (string)TempData["class"];
            string subject = (string)TempData["subject"];
            string teacher = (string)TempData["teacher"];
            reader.Lessons[id].Class = _class;
            reader.Lessons[id].Subject = subject;
            reader.Lessons[id].Teacher = teacher;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}