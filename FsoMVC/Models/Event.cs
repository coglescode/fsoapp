using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FsoMVC.Models;

public class Event
{
  public Guid Id { get; set; }
  
  [StringLength(50)]
  public string Title { get; set; } = string.Empty;

  [DataType(DataType.DateTime)]
  // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTimeOffset StartTime { get; set; } //= DateTimeOffset.UtcNow;

  [DataType(DataType.DateTime)]
  // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTimeOffset EndTime { get; set; } //= DateTimeOffset.UtcNow;

  [MaxLength (100)]
  public string Location { get; set; } = null!;
  
  [MaxLength (200)]
  public string Description { get; set; } = null!;
  
  public bool IsAllDay { get; set; }
  
  public Guid MemberId { get; set; } // Required foreign key
  
  // [System.Text.Json.Serialization.JsonIgnore] 
  // [Newtonsoft.Json.JsonIgnore]

  [ValidateNever] public Member MemberRelated { get; set; } = null!;
  //public Guid? MemberRelated { get; set; }
}