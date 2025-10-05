
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FSO.App.Models;

// Parent model
public class Member
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;

  [Display(Name = "Last Name")]
  public string LastName { get; set; } = null!;

  //public int? SerialNumberId { get; set; } // Nullable to allow for optional members
  //public SerialNumber? SerialNumber { get; set; } // Reference navigation property to the dependent model

  //  public string? Email { get; set; }
  //  public string? Phone { get; set; }
  //  public string? Address { get; set; }
  //  public string? City { get; set; }
  //  public string? State { get; set; }
  //  public string? ZipCode { get; set; }
  //  public DateTimeOffset CreatedAt { get; set; }
  //  public DateTimeOffset UpdatedAt { get; set; }
  //  public override string ToString() => $"{Name} ({Id})";
  //

} 