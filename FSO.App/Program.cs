using System;
using Microsoft.EntityFrameworkCore;

using FSO.App.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddEnvironmentVariables();
           
string? connString = Environment.GetEnvironmentVariable("ConnectionString"); // ?? throw new InvalidOperationException("Connection string not found.");

Console.WriteLine($"ConnectionString is: {connString}");

builder.Services.AddDbContext<FsoAppContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString") ?? throw new InvalidOperationException("Connection string 'FsoAppDbContext' not found.")));


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