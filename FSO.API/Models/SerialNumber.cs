using System.ComponentModel.DataAnnotations.Schema;



namespace FSO.API.Models;

// Dependent model
public class SerialNumber
{
  public Ulid Id { get; set; }

  public string Name { get; set; } = null!; 

  //public Ulid MemberId { get; set; } // Required for the foreign key relationship. Is the member's ID. 

  // [ForeignKey]
  //public Member? Member { get; set; } // Reference navigation property to the parent model

}
