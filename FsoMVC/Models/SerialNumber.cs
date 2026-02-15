using System.ComponentModel.DataAnnotations.Schema;



namespace FsoMVC.Models;

// Dependent model
public class SerialNumber
{
  public int Id { get; set; }

  //public string Name { get; set; } = null!; 

  //public int MemberId { get; set; } // Required for the foreign key relationship. Is the member's ID. 

  // [ForeignKey]
  //public Member? Member { get; set; } // Reference navigation property to the parent model

}
