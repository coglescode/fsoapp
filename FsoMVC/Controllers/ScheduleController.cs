using FsoMVC.Data;
using Microsoft.AspNetCore.Mvc;
using FsoMVC.Models;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

namespace FsoMVC.Controllers;

public class ScheduleController(FsoAppContext context) : Controller
{

  // Get a list of all events including the event's creator
 public async Task<IActionResult> GetData()
  {
    var data = await context.Events
      .Include(e => e.MemberRelated)
      .ToListAsync();

    // Optional: Apply filtering/sorting/paging if needed via dm
    return Json(new {result = data });
  }
  
  // All CRUD actions in one method
  [HttpPost]
  public async Task<IActionResult> CrudActions([FromBody] EditParams data)
  {
    if (data == null) return BadRequest();

    try
    {
      // CREATE
      if (data.Added != null && data.Added.Count > 0)
      {
        foreach (var item in data.Added)
        {
          if (item.Id == Guid.Empty)
            item.Id = Guid.NewGuid();

          context.Events.Add(item);
        }
      }

      // UPDATE
      if (data.Changed != null && data.Changed.Count > 0)
      {
        foreach (var item in data.Changed)
        {
          var existing = await context.Events.FindAsync(item.Id);
          if (existing != null)
          {
            existing.Title = item.Title;
            existing.StartTime = item.StartTime;
            existing.EndTime = item.EndTime;
            existing.IsAllDay = item.IsAllDay;
            existing.Description = item.Description;
            existing.Location = item.Location;
            // existing.RecurrenceRule = item.RecurrenceRule;
            // existing.RecurrenceException = item.RecurrenceException;
            // existing.RecurrenceID = item.RecurrenceID;

            // This line saves your custom Member field (Guid)
            // It uses exactly what the dropdown sent – no new Guid is created
            existing.MemberId = item.MemberId;
            //context.Entry(existing).CurrentValues.SetValues(item);
          }
        }
        await context.SaveChangesAsync();
      }

      // DELETE
      if (data.Deleted != null && data.Deleted.Count > 0)
      {
        foreach (var item in data.Deleted)
        {
          var existing = await context.Events.FindAsync(item.Id);
          if (existing != null)
            context.Events.Remove(existing);
        }
      }

      await context.SaveChangesAsync();

      // Return the affected records (important for Scheduler to update UI)
      return Json(new
      {
        result = data.Added ?? data.Changed ?? data.Deleted
      });
    }
    catch (Exception ex)
    {
      return Json(new { error = ex.Message });
    }
  }

  // Get all existing members to display in a dropdown menu
  public JsonResult GetAllMembers([FromBody] DataManagerRequest dm)
  {
    try
    {
      var members = context.Members
        .Select(m => new
        {
          value = m.Id.ToString(),
          text = string.Concat(m.Name + " ", m.LastName)
        })
        .ToList();
    
      return Json(new { result = members });
    }
    catch (Exception)
    {
      // Log the exception (using a logging framework)
      // Return an error view or message
      return Json(new { message = "An error occurred while loading members." });
    }
  }
}