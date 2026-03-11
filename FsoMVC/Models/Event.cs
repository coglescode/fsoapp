using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FsoMVC.Models;

public class Event
{
  [Key] 
  public Guid Id { get; set; }
  
  [StringLength(50)]
  public string Title { get; set; } = null!;

  [DataType(DataType.Date)] 
  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTime StartDate { get; set; }

  [DataType(DataType.Date)] 
  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTime EndDate { get; set; }

  public string Location { get; set; } = null!;

  public string Description { get; set; } = null!;
  
  public Guid MemberId { get; set; } // Required foreign key
  
  [ValidateNever]
  public Member MemberRelated { get; set; } = null!;
}