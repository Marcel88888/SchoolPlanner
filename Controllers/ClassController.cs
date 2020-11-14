using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public IActionResult AddLesson(Reader reader, int slot, string chosenClass) {
            ViewData["slot"] = slot;
            ViewData["chosen_class"] = chosenClass;
            return View(reader);
        }

        public IActionResult SubmitAddingLesson(Reader reader, int slot, string chosenClass) {
            reader.NewLesson.Class = chosenClass;
            reader.NewLesson.Slot = slot;
            reader.Lessons.Add(reader.NewLesson);
            reader.updateJsonFile();
            return RedirectToAction("Index");       // TODO: Index with class
        }

        public IActionResult EditLesson(Reader reader, int id) {
            ViewData["lessonToEditIndex"] = id;
            return View(reader);
        }

        [HttpPost]
        public IActionResult SubmitEditingLesson(Reader reader, Lesson editedLesson, int id) {
            reader.Lessons[id].Classroom = editedLesson.Classroom;
            reader.Lessons[id].Class = editedLesson.Class;
            reader.Lessons[id].Subject = editedLesson.Subject;
            reader.Lessons[id].Teacher = editedLesson.Teacher;
            reader.updateJsonFile();
            return RedirectToAction("Index");
            // TODO: return RedirectToAction("Index", new { _class = reader.Lessons[id].Class }); 
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