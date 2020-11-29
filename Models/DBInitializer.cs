using System;
using System.Linq;


namespace SchoolPlanner.Models {

    public static class DBInitializer {

        public static void initialize() {
            using (var context = new SchoolPlannerContext()) {
                context.Database.EnsureCreated();

                if (context.Classroom.Any()) {
                    return;   
                }

                context.Classroom.Add(
                    new Classroom {
                        Id = 1,
                        Number = "101",
                    }
                );

                if (context.Class.Any()) {
                    return;   
                }

                context.Class.Add(
                    new Class {
                        Id = 1,
                        Name = "1A",
                    }
                );

                if (context.Subject.Any()) {
                    return; 
                }

                context.Subject.Add(
                    new Subject {
                        Id = 1,
                        Name = "mathematics",
                    }
                );

                if (context.Teacher.Any()) {
                    return;  
                }

                context.Teacher.Add(
                    new Teacher {
                        Id = 1,
                        Surname = "Kowalski",
                    }
                );
                context.SaveChanges();

                if (context.Lesson.Any()) {
                    return;  
                }

                context.Lesson.Add(
                    new Lesson {
                        Id = 1,
                        SubjectId = 1,
                        TeacherId = 1,
                        ClassId = 1,
                        ClassroomId = 1,
                        Slot = 1,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
