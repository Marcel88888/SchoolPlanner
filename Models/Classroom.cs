using System;
using System.ComponentModel.DataAnnotations;


namespace SchoolPlanner.Models {
    public class Classroom {

        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Number { get; set; }

        [Timestamp]
        public DateTime Timestamp { get; set; }
    }
}