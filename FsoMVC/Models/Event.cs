using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FsoMVC.Models;

public class Event
{
  [Key]
  public Guid Id { get; set; }
  public string Title { get; set; } = null!;

  [DataType(DataType.Date)]
  public DateTime StartDate { get; set; }
  
  [DataType(DataType.Date)]
  public DateTime EndDate { get; set; }
  public string Location { get; set; } = null!;
  public string Description { get; set; } = null!;
  // Navigation property for related members
  //public ICollection<Member> Members { get; set; } = new List<Member>();

}
