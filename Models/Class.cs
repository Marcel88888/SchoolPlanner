using System;
using System.ComponentModel.DataAnnotations;


namespace SchoolPlanner.Models {
    public class Class {

        public int Id { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }

        public Class() {}

        public Class(string name) {
            Name = name;
        }
    }
}