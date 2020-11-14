using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;

namespace SchoolPlanner.Controllers {
    public class TeacherController : Controller {
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ILogger<TeacherController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            Reader reader = new Reader();
            return View(reader);
        }

        [HttpPost]
        public IActionResult Index(Reader reader) {     
            ViewData["all_lessons"] = reader.Lessons;
            ViewData["chosen_lessons"] = reader.getLessonsByTeacher(reader.ChosenTeacher);
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader, string ChosenTeacher, int slot) {
            reader.ClassroomsOptions = new List<Classroom>();
            foreach (Classroom classroom in reader.Classrooms) {
                bool classroomAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (classroom.Number == lesson.Classroom && slot == lesson.Slot) {
                        classroomAvailable = false;
                        break;
                    }
                }
                if (classroomAvailable) {
                    reader.ClassroomsOptions.Add(classroom);
                }
            }
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
            ViewData["chosen_teacher"] = ChosenTeacher;
            ViewData["slot"] = slot;
            return View(reader);
        }

        public IActionResult SuccessfulLessonAdding(Reader reader, string ChosenTeacher, int slot) {
            reader.NewLesson = new Lesson();
            string classroom = (string)TempData["classroom"];
            string _class = (string)TempData["class"];
            string subject = (string)TempData["subject"];
            reader.NewLesson.Classroom = classroom;
            reader.NewLesson.Class = _class;
            reader.NewLesson.Teacher = ChosenTeacher;
            reader.NewLesson.Subject = subject;
            reader.NewLesson.Slot = slot;
            reader.Lessons.Add(reader.NewLesson);
            reader.updateJsonFile();
            return View();   
        }

        public IActionResult UnsuccessfulLessonAdding(Reader reader, string chosenTeacher, int slot) {
            ViewData["chosen_teacher"] = chosenTeacher;
            ViewData["slot"] = slot;
            return View();
        }

        public IActionResult EditLesson(Reader reader, int id) {
            reader.ClassroomsOptions = new List<Classroom>();
            Classroom selectedClassroom = new Classroom(reader.Lessons[id].Classroom);
            reader.ClassroomsOptions.Add(selectedClassroom);
            foreach (Classroom classroom in reader.Classrooms) {
                bool classroomAvailable = true;
                foreach (Lesson lesson in reader.Lessons) {
                    if (classroom.Number == lesson.Classroom && reader.Lessons[id].Slot == lesson.Slot) {
                        classroomAvailable = false;
                        break;
                    }
                }
                if (classroomAvailable) {
                    reader.ClassroomsOptions.Add(classroom);
                }
            }
            reader.ClassesOptions = new List<Class>();
            Class selectedClass = new Class(reader.Lessons[id].Class);
            reader.ClassesOptions.Add(selectedClass);
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
            ViewData["lessonToEditIndex"] = id;
            return View(reader);
        }

        public IActionResult SuccessfulLessonEdit(Reader reader, int id) {
            string classroom = (string)TempData["classroom"];
            string subject = (string)TempData["subject"];
            string _class = (string)TempData["class"];
            reader.Lessons[id].Classroom = classroom;
            reader.Lessons[id].Class = _class;
            reader.Lessons[id].Subject = subject;
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

        public IActionResult ValidateAddedData(Reader reader, Lesson newLesson, string chosenTeacher, int slot) {
            if (newLesson.Class == null || newLesson.Classroom == null || newLesson.Subject == null) {
                return RedirectToAction("UnsuccessfulLessonAdding", new { chosenTeacher = chosenTeacher, slot = slot });
            }
            else {
                TempData["classroom"] = newLesson.Classroom;
                TempData["class"] = newLesson.Class;
                TempData["subject"] = newLesson.Subject;
                return RedirectToAction("SuccessfulLessonAdding", new { chosenTeacher = chosenTeacher, slot = slot });
            }
        }

        public IActionResult ValidateEditedData(Reader reader, Lesson editedLesson, int id) {
            if (editedLesson.Class == null || editedLesson.Classroom == null || editedLesson.Subject == null) {
                return RedirectToAction("UnsuccessfulLessonEdit", new { id = id });
            }
            else {
                TempData["classroom"] = editedLesson.Classroom;
                TempData["class"] = editedLesson.Class;
                TempData["subject"] = editedLesson.Subject;
                return RedirectToAction("SuccessfulLessonEdit", new { id = id });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}