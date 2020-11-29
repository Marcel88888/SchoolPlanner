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
        public Classroom Classroom { get; set; }
        [JsonPropertyName("class")]
        public Class Class { get; set; }
        [JsonPropertyName("subject")]
        public Subject Subject { get; set; }
        
        [JsonPropertyName("teacher")]
        public Teacher Teacher { get; set; }

        public Lesson() {}

        public Lesson (Classroom classroom, Class _class, Subject subject, int slot, Teacher teacher) {
            Classroom = classroom;
            Class = _class;
            Subject = subject;
            Slot = slot;
            Teacher = teacher;
        }
    }
}