using Asana.Library.Models;
using System;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Asana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var toDos = new List<ToDo>();
            var projects = new List<Project>();
            int choiceInt;
            var toDoCount = 0;
            var toDoChoice = 0;
            var projChoice = 0;
            var projCount = 0;

            Project proj1 = new Project
            {
                Id = ++projCount,
                Name = "Science Project",
                Description = "Tasks for the science fair"
            };

            Project proj2 = new Project
            {
                Id = ++projCount,
                Name = "Math Homework",
                Description = "Assignments due this week"
            };

            ToDo todo1 = new ToDo
            {
                Id = ++toDoCount,
                Name = "Buy poster board",
                Description = "Go to the store and get materials",
                IsCompleted = false,
                ProjectId = proj1.Id
            };

            ToDo todo2 = new ToDo
            {
                Id = ++toDoCount,
                Name = "Finish worksheet",
                Description = "Chapter 5 algebra problems",
                IsCompleted = false,
                ProjectId = proj2.Id
            };

            // Add projects and todos to the lists
            projects.Add(proj1);
            projects.Add(proj2);
            toDos.Add(todo1);
            toDos.Add(todo2);

            proj1.ToDos.Add(todo1);
            proj2.ToDos.Add(todo2);

            proj1.CompletePercent = CalcProjectCompletion(proj1);
            proj2.CompletePercent = CalcProjectCompletion(proj2);

            do
            {
                Console.WriteLine("Choose a menu option: ");
                Console.WriteLine("1. Create a ToDo ");
                Console.WriteLine("2. List all ToDos");
                Console.WriteLine("3. Delete A ToDo");
                Console.WriteLine("4. Update a ToDo");
                Console.WriteLine("----------------------");
                Console.WriteLine("5. Create a Project");
                Console.WriteLine("6. Delete a Project");
                Console.WriteLine("7. Update a Project");
                Console.WriteLine("8. List all Projects");
                Console.WriteLine("9. List all ToDos in a given Project");
                Console.WriteLine("10. Exit ");

                var choice = Console.ReadLine() ?? "10";

                if (int.TryParse(choice, out choiceInt))
                {

                    switch (choiceInt)
                    {
                        case 1:
                            // Calling for ToDos Info
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var description = Console.ReadLine();

                            // Optional Project Id 
                            projects.ForEach(Console.WriteLine);
                            Console.Write("Enter Project ID (or press Enter to skip): ");
                            string? projInput = Console.ReadLine();

                            int? projId = null;
                            if (int.TryParse(projInput, out int parsedId))
                            {
                                projId = parsedId;
                            }

                            var newToDo = new ToDo
                            {
                                Name = name,
                                Description = description,
                                IsCompleted = false,
                                Id = ++toDoCount,
                                ProjectId = projId
                            };

                            toDos.Add(newToDo);

                            if (projId.HasValue)
                            {
                                var proj = projects.FirstOrDefault(p => p.Id == projId.Value);
                                if (proj != null)
                                {
                                    proj.ToDos.Add(newToDo);
                                    proj.CompletePercent = CalcProjectCompletion(proj);
                                }
                            }

                            break;
                        case 2:
                            toDos.ForEach(Console.WriteLine);
                            break;
                        case 3:
                            toDos.ForEach(Console.WriteLine);
                            Console.WriteLine("ToDo to Delete: ");
                            toDoChoice = int.Parse(Console.ReadLine() ?? "0");

                            var reference = toDos.FirstOrDefault(t => t.Id == toDoChoice);
                            if (reference != null)
                            {
                                toDos.Remove(reference);
                            }

                            if (reference.ProjectId.HasValue)
                            {
                                var proj = projects.FirstOrDefault(p => p.Id == reference.ProjectId.Value);
                                if (proj != null)
                                {
                                    proj.ToDos.RemoveAll(t => t.Id == reference.Id);     // clean removal
                                    proj.CompletePercent = CalcProjectCompletion(proj);  // recalc
                                }
                            }
                            break;
                        case 4:
                            toDos.ForEach(Console.WriteLine);
                            Console.WriteLine("ToDo to Update: ");
                            toDoChoice = int.Parse(Console.ReadLine() ?? "0");
                            var updateReference = toDos.FirstOrDefault(t => t.Id == toDoChoice);

                            if (updateReference != null)
                            {
                                Console.Write("Name (Enter to skip): ");
                                string? newName = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newName))
                                    updateReference.Name = newName;
                                Console.Write("Description (Enter to skip): ");
                                string? newDesc = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newDesc))
                                    updateReference.Description = newDesc;

                                Console.Write("Is this ToDo Complete? (y/n): ");
                                var completed = Console.ReadLine()?.ToLower();
                                if (completed == "y" || completed == "yes")
                                {
                                    updateReference.IsCompleted = true;
                                }
                                else if (completed == "n" || completed == " no")
                                {
                                    updateReference.IsCompleted = false;
                                }
                                if (updateReference.ProjectId.HasValue)
                                {
                                    var proj = projects.FirstOrDefault(p => p.Id == updateReference.ProjectId.Value);
                                    if (proj != null)
                                    {
                                        proj.CompletePercent = CalcProjectCompletion(proj);
                                    }
                                }
                            }
                            break;
                        case 5:   // CREATE A PROJECT
                            Console.Write("Name: ");
                            var projName = Console.ReadLine();
                            Console.Write("Description: ");
                            var projDescription = Console.ReadLine();

                            projects.Add(new Project
                            {
                                Name = projName,
                                Description = projDescription,
                                CompletePercent = 0,
                                Id = ++projCount
                            });
                            break;
                        case 6: // DELETE A PROJECT
                            projects.ForEach(Console.WriteLine);
                            Console.WriteLine("Project to Delete: ");
                            projChoice = int.Parse(Console.ReadLine() ?? "0");

                            var projReference = projects.FirstOrDefault(t => t.Id == projChoice);
                            if (projReference != null)
                            {
                                projects.Remove(projReference);
                            }
                            break;
                        case 7:
                            projects.ForEach(Console.WriteLine);
                            Console.WriteLine("Project to Update: ");
                            projChoice = int.Parse(Console.ReadLine() ?? "0");
                            var projUpdateReference = projects.FirstOrDefault(t => t.Id == projChoice);

                            if (projUpdateReference != null)
                            {
                                Console.Write("Name: ");
                                projUpdateReference.Name = Console.ReadLine();
                                Console.Write("Description: ");
                                projUpdateReference.Description = Console.ReadLine();
                            }
                            break;
                        case 8:
                            projects.ForEach(Console.WriteLine);
                            break;
                        case 9:
                            projects.ForEach(Console.WriteLine);
                            Console.Write("Enter Project ID to view its ToDos: ");
                            int selectedId = int.Parse(Console.ReadLine() ?? "0");

                            var selectedProject = projects.FirstOrDefault(p => p.Id == selectedId);

                            if (selectedProject != null)
                            {
                                if (selectedProject.ToDos.Count == 0)
                                {
                                    Console.WriteLine("No ToDos in this Project.");
                                }
                                else
                                {
                                    Console.WriteLine($"ToDos for project [{selectedProject.Id}] {selectedProject.Name}:");
                                    selectedProject.ToDos.ForEach(Console.WriteLine);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Project not found. ");
                            }
                            break;

                        case 10:
                            break;

                        default:
                            Console.WriteLine("ERROR: Unknown menu selection");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"ERROR: {choice} is not a valid menu selection");
                }

            }
            while (choiceInt != 10);
        }

        private static double CalcProjectCompletion(Project project)
        {
            if (project.ToDos.Count == 0) return 0;
            int completedCount = project.ToDos.Count(t => t.IsCompleted == true);
            return (completedCount * 100.0) / project.ToDos.Count;
        }
    }
}