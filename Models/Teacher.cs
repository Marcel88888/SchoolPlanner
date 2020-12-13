using System;
using System.ComponentModel.DataAnnotations;


namespace SchoolPlanner.Models {
    public class Teacher {

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}