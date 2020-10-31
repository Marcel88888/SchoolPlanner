using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace SchoolPlanner.Models {
    public class Reader {

        public List<int> Classrooms { get; set; }
        public List<string> Classes { get; set; }
        public List<string> Subjects { get; set; }
        public List<string> Teachers { get; set; }

        public Reader() {

            string jsonFilePath = "/home/dell/Studia/sem6/NTR/projects/SchoolPlanner/data/data.json";
            string json = File.ReadAllText(jsonFilePath);

            Classrooms = new List<int>();
            Classes = new List<string>();
            Subjects = new List<string>();
            Teachers = new List<string>();

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement data = doc.RootElement;

            var classrooms = data.GetProperty("classrooms");
            var classes = data.GetProperty("classes");
            var subjects = data.GetProperty("subjects");
            var teachers = data.GetProperty("teachers");
            
            for(int i = 0; i<classrooms.GetArrayLength(); i++) {
                int classroom = Int16.Parse(classrooms[i].GetString());
                Classrooms.Add(classroom);
            }

            for(int i = 0; i<classes.GetArrayLength(); i++) {
                Classes.Add(classes[i].GetString());
            }

            for(int i = 0; i<subjects.GetArrayLength(); i++) {
                Subjects.Add(subjects[i].GetString());
            }

            for(int i = 0; i<teachers.GetArrayLength(); i++) {
                Teachers.Add(teachers[i].GetString());
            }
        }
    }
}