using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FSO.API.Models;

namespace FSO.API.Controllers;

  [Route("api/[controller]")]
  [ApiController]
  public class EventsController : ControllerBase
  {
      private readonly ApiDbContext _context;

      public EventsController(ApiDbContext context)
      {
          _context = context;
      }

      // GET: api/Tasks
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Event>>> GetTasks()
      {
          return await _context.Events.ToListAsync();
      }

      // GET: api/Tasks/5
      [HttpGet("{id}")]
      public async Task<ActionResult<Event>> GetTask(Guid id)
      {
          var task = await _context.Events.FindAsync(id);

          if (task == null)
          {
              return NotFound();
          }

          return task;
      }

      // PUT: api/Tasks/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutTask(Guid id, Event task)
      {
          if (id != task.Id)
          {
              return BadRequest();
          }

          _context.Entry(task).State = EntityState.Modified;

          try
          {
              await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
              if (!TaskExists(id))
              {
                  return NotFound();
              }
              else
              {
                  throw;
              }
          }

          return NoContent();
      }

      // POST: api/Tasks
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Event>> PostTask(Event task)
      {
          _context.Events.Add(task);
          await _context.SaveChangesAsync();

          return CreatedAtAction("GetEvent", new { id = task.Id }, task);
      }

      // DELETE: api/Tasks/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteTask(Guid id)
      {
          var task = await _context.Events.FindAsync(id);
          if (task == null)
          {
              return NotFound();
          }

          _context.Events.Remove(task);
          await _context.SaveChangesAsync();

          return NoContent();
      }

      private bool TaskExists(Guid id)
      {
          return _context.Events.Any(e => e.Id == id);
      }
  }
