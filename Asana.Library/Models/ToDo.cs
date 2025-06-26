

namespace Asana.Library.Models
{
   public class ToDo
{
    public string? Name { get; set; }  // default public property   
    public string? Description { get; set; }
    public bool? Priority { get; set; }
    public int? IsCompleted { get; set; }

        public override string ToString()
        {
            
            return $"{Name} - {Description} ";
        }

}

}



