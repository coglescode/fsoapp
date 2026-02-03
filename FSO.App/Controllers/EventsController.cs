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
      private readonly FsoAppContext _context;

      public EventsController(FsoAppContext context)
      {
          _context = context;
      }

      // GET: Events
      public async Task<IActionResult> Index()
      {
          var events = await _context.Events.ToListAsync();
          
          return View(events);
      }

      // GET: Events/Details/5
      public async Task<IActionResult> Details(Guid id)
      {
        var currentEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
          
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
              _context.Add(currentEvent);
              await _context.SaveChangesAsync();
              //return RedirectToAction(nameof(Index));
              return RedirectToAction("Index", "Home");

          }
          return View(currentEvent);
      }

      // GET: Events/Edit/5
      public async Task<IActionResult> Edit(Guid id)
      {
          if (id == null)
          {
              return NotFound();
          }

          var currenEvent = await _context.Events.FindAsync(id);
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
                  _context.Update(currentEvent);
                  await _context.SaveChangesAsync();
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

          var currentEvent = await _context.Events
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
          var currentEvent = await _context.Events.FindAsync(id);
          if (currentEvent != null)
          {
              _context.Events.Remove(currentEvent);
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
