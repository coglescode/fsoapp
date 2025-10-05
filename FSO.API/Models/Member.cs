using System.ComponentModel.DataAnnotations.Schema;

namespace FSO.API.Models;

//[Table("members", Schema = "fso")]
public class Member{

  public Ulid Id { get; set; }  // Initialize with a new Ulid
  public string? Name { get; set; }
  public string? LastName { get; set; }
  //public SerialNumber? SerialNumber { get; set; } // Reference navigation property to the dependent model


}