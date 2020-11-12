using System;

namespace SchoolPlanner.Models {
    public class Class {
        public string Name { get; set; }

        public Class() {}

        public Class(string name) {
            Name = name;
        }
    }
}