using System;

namespace SchoolPlanner.Models {
    public class Teacher {
        public string Surname { get; set; }

        public Teacher() {}

        public Teacher(string surname) {
            Surname = surname;
        }

        public override string ToString() {
            return Surname;
        }  
    }
}