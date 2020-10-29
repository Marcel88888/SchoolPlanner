using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace SchoolPlanner.Models {
    public class Reader {

        public Reader() {

            string jsonFilePath = "/home/dell/Studia/sem6/NTR/projects/SchoolPlanner/data/data.json";
            string json = File.ReadAllText(jsonFilePath);

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement data = doc.RootElement;

            var classrooms = data.GetProperty("classrooms");
            var classes = data.GetProperty("classes");
            var subjects = data.GetProperty("subjects");
            var teachers = data.GetProperty("teachers");

            Console.WriteLine(classrooms);
            Console.WriteLine(classes);
            Console.WriteLine(subjects);
            Console.WriteLine(teachers);

        }
    }
}