using System.ComponentModel.DataAnnotations;

namespace FsoMVC.Models;

// Parent model
public class Member
{
  [Key] 
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;

  [Display(Name = "Last Name")] 
  public string LastName { get; set; } = null!;
  
  public ICollection<Event> Events { get; } = new List<Event>(); // Member's collection of events
  
  // public Event EventRelated { get; set; } = null!;
}