using System;
using System.ComponentModel.DataAnnotations;


namespace SchoolPlanner.Models {
    public class Class {

        public int Id { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}