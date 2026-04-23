using System.Diagnostics;
using FsoMVC.Models;
using FsoMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;



namespace FsoMVC.Controllers;

public class HomeController(FsoAppContext context) : Controller
{
  public ActionResult Index()
  {
    return View();
  }

  public JsonResult LoadData()
  {
    // var data = GetAllEvents();
    var data = context.Events
      .Include(e => e.MemberRelated)
      .ToList();
    // Console.WriteLine("Events are:" + string.Join(",", data));

    return Json(data);
  }

  public List<Event> ListEvents()
  {
    var events = context.Events
      .Select(e => new Event
      {
        Id = e.Id,
        Title = e.Title,
        StartTime = e.StartTime, //.UtcDateTime.ToLocalTime(),
        EndTime = e.EndTime, //.UtcDateTime.ToLocalTime(),
        Location = e.Location,
        Description = e.Description,
        MemberRelated = e.MemberRelated,
      }).ToList();

    return events;
  }

  


  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
    
  