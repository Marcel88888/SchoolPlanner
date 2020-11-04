using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolPlanner.Models;

namespace SchoolPlanner.Controllers {
    public class EditController : Controller {
        private readonly ILogger<HomeController> _logger;

        public EditController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            Edit edit = new Edit();
            Reader reader = new Reader();
            return View(edit);
        }

        [HttpPost]
        public IActionResult Index(Edit edit, Reader reader) {
            if (edit.ClassroomToAdd != null) {
                reader.Classrooms.Add(new Classroom(edit.ClassroomToAdd));
                foreach(Classroom classroom in reader.Classrooms) {
                    Console.WriteLine(classroom.Number);
                }
            }
            return View(edit);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
