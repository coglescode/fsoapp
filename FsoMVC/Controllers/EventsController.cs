using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FsoMVC.Data;
using FsoMVC.Models;

namespace FsoMVC.Controllers;

  public class EventsController(FsoAppContext context) : Controller
  {
      // GET: Events
      public async Task<IActionResult> Index()
      {
          var events = await context.Events.ToListAsync();
          
          return View(events);
      }

      // GET: Events/Details/5
      public async Task<IActionResult> Details(Guid id)
      {
        var currentEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == id);
          
        if (currentEvent == null)
        {
            return NotFound();
        }

        return View(currentEvent);
      }

      // GET: Events/Create
      public IActionResult Create()
      {
          return View();
      }

      // POST: Events/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Title,StartDate,EndDate,Location,Description")] Event currentEvent)
      {
          if (ModelState.IsValid)
          {
              currentEvent.Id = Guid.NewGuid();
              context.Add(currentEvent);
              await context.SaveChangesAsync();
              //return RedirectToAction(nameof(Index));
              return RedirectToAction("Index", "Home");

          }
          return View(currentEvent);
      }

      // GET: Events/Edit/5
      public async Task<IActionResult> Edit(Guid id)
      {
          // if (id == null)
          // {
          //     return NotFound();
          // }

          var currenEvent = await context.Events.FindAsync(id);
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
      public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,StartDate,EndDate,Location,Description")] Event currentEvent)
      {
          if (id != currentEvent.Id)
          {
              return NotFound();
          }

          if (ModelState.IsValid)
          {
              try
              {
                  context.Update(currentEvent);
                  await context.SaveChangesAsync();
              }
              catch (DbUpdateConcurrencyException)
              {
                  if (!EventExists(currentEvent.Id))
                  {
                      return NotFound();
                  }
                  else
                  {
                      throw;
                  }
              }
              return RedirectToAction(nameof(Index));
          }
          return View(currentEvent);
      }

      // GET: Events/Delete/5
      public async Task<IActionResult> Delete(Guid id)
      {
          // if (id == null)
          // {
          //     return NotFound();
          // }

          var currentEvent = await context.Events
              .FirstOrDefaultAsync(m => m.Id == id);
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
          var currentEvent = await context.Events.FindAsync(id);
          if (currentEvent != null)
          {
              context.Events.Remove(currentEvent);
          }

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
            e.Description
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
