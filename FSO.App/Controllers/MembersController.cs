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

  public class MembersController : Controller
  {
      private readonly FSOAppContext _context;

      public MembersController(FSOAppContext context)
      {
          _context = context;
      }

      // GET: Members
      public async Task<IActionResult> Index()
      {
        var members = await _context.Members.ToListAsync();
   
        return View(members);

      }

      // GET: Members/Details/5
      public async Task<IActionResult> Details(Guid id)
      {
        //if (id == null)
        //{
        //    return NotFound();
        //}

        var member = await _context.Members
        .FirstOrDefaultAsync(m => m.Id == id);
          if (member == null)
          {
              return NotFound();
          }

          return View(member);
      }

      // GET: Members/Create
      public IActionResult Create()
      {
          return View();
      }

      // POST: Members/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Name,LastName")] Member member)
      {
          if (ModelState.IsValid)
          {
              _context.Add(member);
              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
          }
          return View(member);
      }
  
      // GET: Members/Edit/5
      public async Task<IActionResult> Edit(Guid id)
      {
        //if (id == null)
        //{
        //    return NotFound();
        //}

        var member = await _context.Members.FindAsync(id);
        if (member == null)
        {
          return NotFound();
        }
        return View(member);
      }

  // POST: Members/Edit/5
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,LastName")] Member member)
  {
      if (id != member.Id)
      {
          return NotFound();
      }

      if (ModelState.IsValid)
      {
          try
          {
              _context.Update(member);
              await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
              if (!MemberExists(member.Id))
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
      return View(member);
  }

      // GET: Members/Delete/5
      public async Task<IActionResult> Delete(Guid id)
      {
          //if (id == null)
          //{
          //    return NotFound();
          //}

          var member = await _context.Members
              .FirstOrDefaultAsync(m => m.Id == id);
          if (member == null)
          {
              return NotFound();
          }

        return View(member);
        //return RedirectToAction(nameof(Index));
      }

      // POST: Members/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(Guid id)
      {
          var member = await _context.Members.FindAsync(id);
          if (member != null)
          {
              _context.Members.Remove(member);
          }

          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
      }

      private bool MemberExists(Guid id)
      {
          return _context.Members.Any(e => e.Id == id);
      }

    public JsonResult GetAllMembers()
    {

      try
      {
        var members = _context.Members
          .Select(m => new
          {
            m.Id,
            m.Name,
            m.LastName
          })
          .ToList();

          return Json(members);

      }
      catch (Exception)
      {
        // Log the exception (using a logging framework)
        // Return an error view or message
        return Json(new { message = "An error occurred while loading bookings." });
      }
    }



  }
