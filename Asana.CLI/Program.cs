using Asana.Library.Models;
using System;

namespace Asana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? myName = null;

            Console.WriteLine($"Hello, {myName ?? "Bob" }!");
            // ?? --> means "if everything on the left had in null do something different. 
        }
    }
}