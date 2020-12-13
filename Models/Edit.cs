using System;

namespace SchoolPlanner.Models {
    public class Edit {
        public string ClassroomToAdd { get; set; }
        public int? ClassroomToDelete { get; set; }
        public string ClassToAdd { get; set; }
        public int? ClassToDelete { get; set; }
        public string SubjectToAdd { get; set; }
        public int? SubjectToDelete { get; set; }
        public string TeacherToAdd { get; set; }
        public int? TeacherToDelete { get; set; }
    }
}