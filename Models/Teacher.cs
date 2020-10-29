using System;

namespace SchoolPlanner.Models {
    public class Teacher {
        public string surname { get; set; }

        public Teacher(string surn) {
            surname = surn;
        }
    }
}