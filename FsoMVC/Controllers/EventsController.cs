using FsoMVC.Data;
using FsoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FsoMVC.Controllers;

public class EventsController(FsoAppContext context) : Controller
{
  // GET: Events
  public async Task<IActionResult> Index()
  {
    var events = context.Events
      .Include(e => e.MemberRelated)
      .OrderBy(e => e.StartDate);
    
    return View(await events.ToListAsync());
  }

  // GET: Events/Details/5
  public async Task<IActionResult> Details(Guid id)
  {
    var currentEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == id);

    if (currentEvent == null) return NotFound();

    return View(currentEvent);
  }

  // GET: Events/Create
  public IActionResult Create()
  {
    ViewData["MemberId"] = new SelectList(context.Members, "Id", "Name");
    
    return View();
  }

  // POST: Events/Create
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([Bind("Title,StartDate,EndDate,Location,Description,MemberId")] Event currentEvent)
  {
    if (ModelState.IsValid)
    {
      currentEvent.Id = Guid.NewGuid();
      
      context.Add(currentEvent);
      await context.SaveChangesAsync();
      
      return RedirectToAction(nameof(Index));
    }
    return View(currentEvent);
  }

  // GET: Events/Edit/5
  public async Task<IActionResult> Edit(Guid id)
  {
    ViewData["MemberId"] = new SelectList(context.Members, "Id", "Name");
    
    var currenEvent = await context.Events
      .FindAsync(id);
      
    if (currenEvent == null)
    {
      return NotFound();
    }

    return View(currenEvent);
  }

  // POST: Events/Edit/5
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(Guid? id,[Bind("Id,Title,StartDate,EndDate,Location,Description, MemberId")] Event eventId)
  {
    if (id != eventId.Id)
    {
      return NotFound();
    }

    var eventToUpdate = await context.Events.FirstOrDefaultAsync(m => m.Id == id);
   
    if (ModelState.IsValid)
    {
      try
      {
        await context.Events
        .Where(e => e.Id == id)
          .ExecuteUpdateAsync(e => e
            .SetProperty(b => b.Title, eventId.Title)
            .SetProperty(b => b.StartDate, eventId.StartDate)
            .SetProperty(b => b.EndDate, eventId.EndDate)
            .SetProperty(b => b.Location, eventId.Location)
            .SetProperty(b => b.Description, eventId.Description)
            .SetProperty(b => b.MemberId, eventId.MemberId)
          );
        
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!EventExists(eventId.Id)) return NotFound();

        throw;
      }
      return RedirectToAction(nameof(Index));
    }
    return View(eventId);
  }

  
  // GET: Events/Delete/5
  public async Task<IActionResult> Delete(Guid id)
  {
    var currentEvent = await context.Events.FirstOrDefaultAsync(m => m.Id == id);
    
    if (currentEvent == null)
    {
      return NotFound();
    }

    return View(currentEvent);
  }

  // POST: Events/Delete/5
  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(Guid id)
  {
    var thisEvent = await context.Events.FindAsync(id);
    
    if (thisEvent == null) return NotFound();
    await context.Events
      .Where(e => e.Id == id)
      .ExecuteDeleteAsync();
    
    await context.SaveChangesAsync();
    
    return RedirectToAction(nameof(Index));
  }

  private bool EventExists(Guid id)
  {
    return context.Events.Any(e => e.Id == id);
  }

  public JsonResult GetAllEvents()
  {
    try
    {
      var events = context.Events
        .Select(e => new
        {
          e.Id,
          e.Title,
          e.StartDate,
          e.EndDate,
          e.Location,
          e.Description,
          e.MemberRelated.Name
        })
        .OrderBy(e => e.StartDate)
        .ToList();

      Console.WriteLine("The events are:" + string.Join(",", events));

      return Json(events);
    }
    catch (Exception)
    {
      // Log the exception (using a logging framework)
      // Return an error view or message
      return Json(new { message = "An error occurred while loading bookings." });
    }
  }
}