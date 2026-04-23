namespace FsoMVC.Models;

public class EditParams
{
  public string Action { get; } = string.Empty;
  public List<Event> Added { get; set; } = new();
  public List<Event>? Changed { get; set; }
  public List<Event> Deleted { get; set; } = new();
  public Event? Value { get; set; }
  
  public string? Key { get; set; }

  // public EditParams(string action, List<Event> added, List<Event> changed, List<Event> deleted, Event value)
  // {
  //   Action = action;
  //   Added = added;
  //   Changed = changed;
  //   Deleted = deleted;
  //   Value = value;
  //   
  // }
}