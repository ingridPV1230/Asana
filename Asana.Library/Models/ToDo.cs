

namespace Asana.Library.Models

{
   public class ToDo
{
    private string? name;
    // public string? Name { get; set; }  // default public property  
    public string? Name  // needs the private on l7
    {
        get
        {
            return name;
        }

        set
        {
            if (name != value)
            {
                name = value;
            }
        }
    }
    
    public string? Description { get; set; }
    public bool? IsDone { get; set; }
    public int? Priority { get; set; }

    public ToDo()
    {

    }

}

}



