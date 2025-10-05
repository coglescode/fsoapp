using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FSO.App.Data;
using FSO.App.Models;

namespace FSO.App.Controllers;

  public class EventsController : Controller
  {
      private readonly FSOAppContext _context;

      public EventsController(FSOAppContext context)
      {
          _context = context;
      }

      // GET: Events
      public async Task<IActionResult> Index()
      {
          return View(await _context.Events.ToListAsync());
      }

      // GET: Events/Details/5
      public async Task<IActionResult> Details(Guid? id)
      {
          if (id == null)
          {
              return NotFound();
          }

          var @event = await _context.Events
              .FirstOrDefaultAsync(m => m.Id == id);
          if (@event == null)
          {
              return NotFound();
          }

          return View(@event);
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
      public async Task<IActionResult> Create([Bind("Title,StartDate,EndDate,Location,Description")] Event @event)
      {
          if (ModelState.IsValid)
          {
              @event.Id = Guid.NewGuid();
              _context.Add(@event);
              await _context.SaveChangesAsync();
              //return RedirectToAction(nameof(Index));
              return RedirectToAction("Index", "Home");

          }
          return View(@event);
      }

      // GET: Events/Edit/5
      public async Task<IActionResult> Edit(Guid? id)
      {
          if (id == null)
          {
              return NotFound();
          }

          var @event = await _context.Events.FindAsync(id);
          if (@event == null)
          {
              return NotFound();
          }
          return View(@event);
      }

      // POST: Events/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(Guid id, [Bind("Title,StartDate,EndDate,Location,Description")] Event @event)
      {
          if (id != @event.Id)
          {
              return NotFound();
          }

          if (ModelState.IsValid)
          {
              try
              {
                  _context.Update(@event);
                  await _context.SaveChangesAsync();
              }
              catch (DbUpdateConcurrencyException)
              {
                  if (!EventExists(@event.Id))
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
          return View(@event);
      }

      // GET: Events/Delete/5
      public async Task<IActionResult> Delete(Guid? id)
      {
          if (id == null)
          {
              return NotFound();
          }

          var @event = await _context.Events
              .FirstOrDefaultAsync(m => m.Id == id);
          if (@event == null)
          {
              return NotFound();
          }

          return View(@event);
      }

      // POST: Events/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(Guid id)
      {
          var @event = await _context.Events.FindAsync(id);
          if (@event != null)
          {
              _context.Events.Remove(@event);
          }

          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
      }

      private bool EventExists(Guid id)
      {
          return _context.Events.Any(e => e.Id == id);
      }

    public JsonResult GetAllEvents()
    {
      try
      {
        var events = _context.Events
          .Select(e => new
          {
            e.Id,
            e.Title,
            e.StartDate,
            e.EndDate,
            e.Location,
            e.Description
          })
          .ToList();

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
