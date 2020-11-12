using System.Text.Json.Serialization;

namespace SchoolPlanner.Models {
    public class Lesson {
        [JsonPropertyName("classroom")]
        public string Classroom { get; set; }
        [JsonPropertyName("class")]
        public string Class { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        [JsonPropertyName("slot")]
        public int Slot { get; set; }
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