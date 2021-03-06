using System.ComponentModel.DataAnnotations;
using System;


namespace SchoolPlanner.Models {
    public class Lesson {

        public int Id { get; set; }

        [Required]
        public virtual Classroom Classroom { get; set; }

        [Required]
        public virtual Class Class { get; set; }

        [Required]
        public virtual Subject Subject { get; set; }

        [Required]
        public int Slot { get; set; }

        [Required]
        public virtual Teacher Teacher { get; set; }
        
        [Timestamp]
        public DateTime Timestamp { get; set; }
    }
}