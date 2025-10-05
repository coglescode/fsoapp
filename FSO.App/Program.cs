using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FSO.App.Data;
namespace FSO.App
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllersWithViews();
      builder.Configuration.AddEnvironmentVariables();
           
      //string? connString = Environment.GetEnvironmentVariable("ConnectionString") ?? throw new InvalidOperationException("Connection string not found.");

      //builder.Services.AddDbContext<FSOAppContext>(options =>
      //    options.UseNpgsql(connString));

      builder.Services.AddDbContext<FSOAppContext>(options =>
          options.UseNpgsql("Members"));


      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseRouting();

      app.UseAuthorization();

      app.MapStaticAssets();
      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}")
          .WithStaticAssets();

      app.Run();
    }
  }
}
