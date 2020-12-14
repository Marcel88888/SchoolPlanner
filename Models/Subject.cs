using System;
using System.ComponentModel.DataAnnotations;


namespace SchoolPlanner.Models {
    public class Subject {

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Timestamp]
        public DateTime Timestamp { get; set; }
    }
}