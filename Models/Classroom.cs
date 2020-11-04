using System;

namespace SchoolPlanner.Models {
    public class Classroom {
        public string Number { get; set; }

        public Classroom() {}

        public Classroom(string number) {
            Number = number;
        }
    }
}