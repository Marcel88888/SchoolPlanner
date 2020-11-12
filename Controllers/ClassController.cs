using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Index(Reader reader) {
            ViewData["chosen_class"] = reader.ChosenClass;
            ViewData["chosen_lessons"] = reader.getLessonsByClass(reader.ChosenClass);
            Console.WriteLine("juhu");
            return View(reader);
        }

        public IActionResult AddLesson(Reader reader) {
            return View(reader);
        }

        public IActionResult EditLesson(Reader reader) {
            ViewData["lessonToEditIndex"] = reader.ChosenLessonIndex;
            Console.WriteLine("AAA");
            return View(reader);
        }

        [HttpPost]
        public IActionResult SubmitEditingLesson(Reader reader) {
            Console.WriteLine("#####################");
            Console.WriteLine("BBB");
            foreach (Lesson lesson in reader.Lessons) {
                Console.WriteLine(lesson.Subject);
            }
            reader.updateJsonFile();
            Console.WriteLine("-------------------------");
            foreach (Lesson lesson in reader.Lessons) {
                Console.WriteLine(lesson.Subject);
            }
            return RedirectToAction("Index", new { reader = reader });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}