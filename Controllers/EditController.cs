using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;
using System;

namespace SchoolPlanner.Controllers {
    public class EditController : Controller {
        private readonly ILogger<HomeController> _logger;

        public EditController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            Edit edit = new Edit();
            Reader reader = new Reader();
            edit._Reader = reader;
            return View(edit);
        }

        [HttpPost]
        public IActionResult Index(Edit edit, Reader reader) {
            if (edit.ClassroomToAdd != null) {
                reader.Classrooms.Add(new Classroom(edit.ClassroomToAdd));
                reader.updateJsonFile();
            }
            if (edit.ClassroomToDelete != null) {
                reader.Classrooms.RemoveAll(x => x.Number == edit.ClassroomToDelete);
                reader.updateJsonFile();
            }
            if (edit.ClassToAdd != null) {
                reader.Classes.Add(new _Class(edit.ClassToAdd));
                reader.updateJsonFile();
            }
            if (edit.ClassToDelete != null) {
                reader.Classes.RemoveAll(x => x.Name == edit.ClassToDelete);
                reader.updateJsonFile();
            }
            if (edit.SubjectToAdd != null) {
                reader.Subjects.Add(edit.SubjectToAdd);
                reader.updateJsonFile();
            }
            if (edit.SubjectToDelete != null) {
                reader.Subjects.RemoveAll(x => x == edit.SubjectToDelete);
                reader.updateJsonFile();
            }
            if (edit.TeacherToAdd != null) {
                reader.Teachers.Add(new Teacher(edit.TeacherToAdd));
                reader.updateJsonFile();
            }
            if (edit.TeacherToDelete != null) {
                reader.Teachers.RemoveAll(x => x.Surname == edit.TeacherToDelete);
                reader.updateJsonFile();
            }
            edit._Reader = reader;
            return View(edit);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
