using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SchoolPlanner.Models {
    public class Lesson {

        public int Id { get; set; }

        [Required]
        public int ClassroomId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [Required]
        public int TeacherId { get; set; }



        [JsonPropertyName("classroom")]
        public string Classroom { get; set; }
        [JsonPropertyName("class")]
        public string Class { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        
        [JsonPropertyName("teacher")]
        public string Teacher { get; set; }

        public Lesson() {}

        public Lesson (string classroom, string _class, string subject, int slot, string teacher) {
            Classroom = classroom;
            Class = _class;
            Subject = subject;
            Slot = slot;
            Teacher = teacher;
        }
    }
}