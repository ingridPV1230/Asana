using System.Collections.Generic;

namespace Asana.Library.Models
{
    public class Project
    {
        public string? Name { get; set; }  // default public property   
        public string? Description { get; set; }
        public List<ToDo> ToDos { get; set; } = new List<ToDo>();
        public double CompletePercent { get; set; }

        public int Id { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Description} - Completion: {CompletePercent}%";
        }

    }

}