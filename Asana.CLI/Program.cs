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
            do
            {
                Console.WriteLine("Choose a menu option: ");
                Console.WriteLine("1. Create a ToDo ");
                Console.WriteLine("2. Exit ");

                var choice = Console.ReadLine() ?? "2";

                if (int.TryParse(choice, out choiceInt))
                {

                    switch (choiceInt)
                    {
                        case 1:
                            var toDo = new ToDo();
                            Console.Write("Name: ");
                            toDo.Name = Console.ReadLine();
                            Console.Write("Description: ");
                            toDo.Description = Console.ReadLine();

                            toDos.Add(toDo);
                            break;
                        case 2:
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
                if (toDos.Any())
                {
                   Console.WriteLine(toDos.Last()); 
                }  
            }
            while (choiceInt != 2);       
        } 
    }
}