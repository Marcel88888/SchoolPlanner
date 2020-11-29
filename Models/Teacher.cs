using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolPlanner.Models {
    public class Teacher {

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }
        
        public DateTime Timestamp { get; set; }

        public Teacher() {}

        public Teacher(string surname) {
            Surname = surname;
        }

        public override string ToString() {
            return Surname;
        }  
    }
}