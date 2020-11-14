using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SchoolPlanner.Models;


namespace SchoolPlanner.Controllers {
    public class ClassController : Controller {
        private readonly ILogger<ClassController> _logger;

        public ClassController(ILogger<ClassController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            Reader reader = new Reader();
            return View(reader);
        }

        [HttpPost]
        public IActionResult Index(Reader reader) {     // TODO: add "string _class" as a parameter 
            ViewData["all_lessons"] = reader.Lessons;
            ViewData["chosen_lessons"] = reader.getLessonsByClass(reader.ChosenClass);
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader, string chosenClass, int slot) {
            reader.ClassroomsOptions = new List<Classroom>();
            foreach (Classroom classroom in reader.Classrooms) {
                bool classroomAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (classroom.Number == lesson.Classroom && slot == lesson.Slot) {
                        classroomAvailable = false;
                    }
                }
                if (classroomAvailable) {
                    reader.ClassroomsOptions.Add(classroom);
                }
            }
            reader.TeachersOptions = new List<Teacher>();
            foreach (Teacher teacher in reader.Teachers) {
                bool teacherAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (teacher.Surname == lesson.Teacher && slot == lesson.Slot) {
                        teacherAvailable = false;
                    }
                }
                if (teacherAvailable) {
                    reader.TeachersOptions.Add(teacher);
                }
            }
            ViewData["chosen_class"] = chosenClass;
            ViewData["slot"] = slot;
            return View(reader);
        }

        public IActionResult SuccessfulLessonAdding(Reader reader, string chosenClass, int slot) {
            reader.NewLesson = new Lesson();
            string classroom = (string)TempData["classroom"];
            string subject = (string)TempData["subject"];
            string teacher = (string)TempData["teacher"];
            reader.NewLesson.Classroom = classroom;
            reader.NewLesson.Class = chosenClass;
            reader.NewLesson.Subject = subject;
            reader.NewLesson.Slot = slot;
            reader.NewLesson.Teacher = teacher;
            reader.Lessons.Add(reader.NewLesson);
            reader.updateJsonFile();
            return View();       // TODO: Index with class
        }

        public IActionResult UnsuccessfulLessonAdding(Reader reader, string chosenClass, int slot) {
            Console.WriteLine("Chosen class: " + chosenClass);
            Console.WriteLine("Slot: " + slot.ToString());
            ViewData["chosen_class"] = chosenClass;
            ViewData["slot"] = slot;
            return View();
        }

        public IActionResult EditLesson(Reader reader, int id) {
            reader.ClassroomsOptions = new List<Classroom>();
            foreach (Classroom classroom in reader.Classrooms) {
                bool classroomAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (classroom.Number == lesson.Classroom && reader.Lessons[id].Slot == lesson.Slot) {
                        classroomAvailable = false;
                    }
                }
                if (classroomAvailable) {
                    reader.ClassroomsOptions.Add(classroom);
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
            string classroom = (string)TempData["classroom"];
            string subject = (string)TempData["subject"];
            string teacher = (string)TempData["teacher"];
            reader.Lessons[id].Classroom = classroom;
            reader.Lessons[id].Subject = subject;
            reader.Lessons[id].Teacher = teacher;
            reader.updateJsonFile();
            return View();
            // TODO: return RedirectToAction("Index", new { _class = reader.Lessons[id].Class }); 
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

        public IActionResult ValidateAddedData(Reader reader, Lesson newLesson, string chosenClass, int slot) {
            if (newLesson.Classroom == null || newLesson.Subject == null || newLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenClass = chosenClass, slot = slot });
            }
            else {
                TempData["classroom"] = newLesson.Classroom;
                TempData["subject"] = newLesson.Subject;
                TempData["teacher"] = newLesson.Teacher;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenClass = chosenClass, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Reader reader, Lesson editedLesson, int id) {
            if (editedLesson.Classroom == null || editedLesson.Subject == null || editedLesson.Teacher == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["classroom"] = editedLesson.Classroom;
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