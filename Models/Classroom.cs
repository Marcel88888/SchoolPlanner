using System;

namespace SchoolPlanner.Models {
    public class Classroom {
        public int Number { get; set; }

        public Classroom(int number) {
            Number = number;
        }
    }
}