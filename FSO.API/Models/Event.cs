using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FSO.API.Models
{
    public class Event
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Ulid Id { get; set; }
        public string? Title {get; set;}
        //public DateTime StartDate {get; set;}
        //public DateTime EndDate {get; set;}

        //public List<Tasks>
    }
}
