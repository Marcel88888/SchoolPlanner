using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace SchoolPlanner.Models {
    
    public class JsonData {

        [JsonPropertyName("classrooms")]
        public List<string> Classrooms { get; set; }
        [JsonPropertyName("classes")]
        public List<string> Classes { get; set; }
        [JsonPropertyName("subjects")]
        public List<string> Subjects { get; set; }
        [JsonPropertyName("teachers")]
        public List<string> Teachers { get; set; }
        [JsonPropertyName("lessons")]
        public List<Lesson> Lessons { get; set; }

        public JsonData(List<string> classrooms, List<string> classes, List<string> subjects, List<string> teachers, List<Lesson> lessons) {
            Classrooms = classrooms;
            Classes = classes;
            Subjects = subjects;
            Teachers = teachers;
            Lessons = lessons; 
        }
    }
}