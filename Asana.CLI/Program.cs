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
            int choiceInt;
            var itemCount = 0;
            do
            {
                Console.WriteLine("Choose a menu option: ");
                Console.WriteLine("1. Create a ToDo ");
                Console.WriteLine("2. List all ToDos");
                Console.WriteLine("3. List all outstanding ToDos");     // not done
                Console.WriteLine("4. Delete A ToDo");
                Console.WriteLine("5. Exit ");

                var choice = Console.ReadLine() ?? "5";

                if (int.TryParse(choice, out choiceInt))
                {

                    switch (choiceInt)
                    {
                        case 1:
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var description = Console.ReadLine();

                            toDos.Add(new ToDo
                            {
                                Name = name,
                                Description = description,
                                IsCompleted = false,
                                Id = ++itemCount
                            });
                            break;
                        case 2:
                            toDos.ForEach(Console.WriteLine);
                            break;
                        case 3:
                            toDos.Where(t => (t != null) && !(t?.IsCompleted ?? false))
                            .ToList()
                            .ForEach(Console.WriteLine);
                            break;
                        case 4:
                            toDos.ForEach(Console.WriteLine);
                            Console.WriteLine("ToDo to Delete: ");
                            var toDoChoice = int.Parse(Console.ReadLine() ?? "0");

                            var reference = toDos.FirstOrDefault(t => t.Id == toDoChoice);
                            if (reference != null)
                            {
                                toDos.Remove(reference);
                            }

                            break;
                        case 5:
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
            while (choiceInt != 5);       
        } 
    }
}