using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolPlanner.Models {
    public class Subject {

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        
        public DateTime Timestamp { get; set; }

        public Subject(string name) {
            Name = name;
        }
    }
}