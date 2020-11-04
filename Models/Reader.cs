using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace SchoolPlanner.Models {
    public class Reader {

        public List<Classroom> Classrooms { get; set; }
        public List<_Class> Classes { get; set; }
        public List<string> Subjects { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Lesson> Lessons { get; set; }
        public string Chosen_class { get; set; }

        public Reader() {

            string jsonFilePath = "./data/data.json";
            string json = File.ReadAllText(jsonFilePath);

            Classrooms = new List<Classroom>();
            Classes = new List<_Class>();
            Subjects = new List<string>();
            Teachers = new List<Teacher>();
            Lessons = new List<Lesson>();

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement data = doc.RootElement;

            var classrooms = data.GetProperty("classrooms");
            var classes = data.GetProperty("classes");
            var subjects = data.GetProperty("subjects");
            var teachers = data.GetProperty("teachers");
            var lessons = data.GetProperty("lessons");
            
            for (int i=0; i<classrooms.GetArrayLength(); i++) {
                string classroom = classrooms[i].GetString();
                Classrooms.Add(new Classroom(classroom));
            }

            for (int i=0; i<classes.GetArrayLength(); i++) {
                Classes.Add(new _Class(classes[i].GetString()));
            }

            for (int i=0; i<subjects.GetArrayLength(); i++) {
                Subjects.Add(subjects[i].GetString());
            }

            for (int i=0; i<teachers.GetArrayLength(); i++) {
                Teachers.Add(new Teacher(teachers[i].GetString()));
            }

            // finding objects with the same data as in lessons
            for (int i=0; i<lessons.GetArrayLength(); i++) {

                Classroom found_classroom = new Classroom();
                _Class found_class = new _Class();
                string found_subject = null; 
                Teacher found_teacher = new Teacher();

                // finding classrooms
                var cl_room = lessons[i].GetProperty("classroom").GetString();
                for (int j=0; j<classrooms.GetArrayLength(); j++) {
                    string cl_room2 = Classrooms[j].Number;
                    if (string.Equals(cl_room, cl_room2)) {
                        found_classroom = Classrooms[j];
                        break;
                    }
                }
                // finding classes
                var cl = lessons[i].GetProperty("class").GetString();
                for (int j=0; j<classes.GetArrayLength(); j++) {
                    string cl2 = Classes[j].Name;
                    if (string.Equals(cl, cl2)) {
                        found_class = Classes[j];
                        break;
                    }
                } 
                // finding subjects
                var subj = lessons[i].GetProperty("subject").GetString();
                for (int j=0; j<subjects.GetArrayLength(); j++) {
                    string subj2 = Subjects[j];
                    if (string.Equals(subj, subj2)) {
                        found_subject = Subjects[j];
                        break;
                    }
                }  
                // finfing teachers
                var t = lessons[i].GetProperty("teacher").GetString();
                for (int j=0; j<teachers.GetArrayLength(); j++) {
                    string t2 = Teachers[j].Surname;
                    if (string.Equals(t, t2)) {
                        found_teacher = Teachers[j];
                        break;
                    }
                }   
                int slot = lessons[i].GetProperty("slot").GetInt16();
                if (found_classroom!=null && found_class!=null && !String.IsNullOrEmpty(found_subject) && found_teacher!=null) {
                    Lessons.Add(new Lesson(found_classroom, found_class, found_subject, slot, found_teacher));
                }
                else {
                    Console.WriteLine("Error: Lesson defined in the json file contains data that is not defined in the previous sections of the file");
                }
            }
        }

        public List<Lesson> getLessonsByClass(string _class) { // TODO: change for (__Class _class) (but then @Html.DropDownListFor in Index.cshtml is not working)
            List<Lesson> chosen_lessons = new List<Lesson>();
            Console.WriteLine("UUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU");
            foreach (Lesson lesson in Lessons) {
                if (lesson._Class.Name.Equals(_class)) {
                    Console.WriteLine("tak");
                    chosen_lessons.Add(lesson);
                    Console.WriteLine(lesson.Slot);
                }
                else {
                    Console.WriteLine("nie");
                }
            }
            return chosen_lessons;
        }
    }
}