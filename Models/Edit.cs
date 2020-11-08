using System;

namespace SchoolPlanner.Models {
    public class Edit {
        public string ClassroomToAdd { get; set; }
        public string ClassroomToDelete { get; set; }
        public string ClassToAdd { get; set; }
        public string ClassToDelete { get; set; }
        public string SubjectToAdd { get; set; }
        public string SubjectToDelete { get; set; }
        public string TeacherToAdd { get; set; }
        public string TeacherToDelete { get; set; }
        public Reader _Reader { get; set; }
    }
}