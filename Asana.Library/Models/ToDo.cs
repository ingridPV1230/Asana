
namespace Asana.Library.Models
{
    public class ToDo
    {
        public string? Name { get; set; }  // default public property   
        public string? Description { get; set; }
        public bool? Priority { get; set; }
        public bool? IsCompleted { get; set; }

        public int Id { get; set; }

        public int? ProjectId { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Description} - ProjectId: {ProjectId}";
        }
    }
}
