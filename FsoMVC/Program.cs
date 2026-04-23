using FsoMVC.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddEnvironmentVariables();
// builder.Services.AddScoped<EventsDataAdaptor>();
// builder.Services.AddScoped<FsoAppContext>();

builder.Services.AddControllersWithViews()
  .AddJsonOptions(options => {
    // This stops .NET 10 from turning "Subject" into "subject"
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
  });

builder.Services.AddControllers()
  .AddNewtonsoftJson(options =>
  {
    options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // PascalCase
    options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
  });

builder.Services.AddDbContext<FsoAppContext>(options =>
  options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString") ??
                    throw new InvalidOperationException("Connection string 'FsoAppDbContext' not found.")));
  
builder.Services.AddControllers()
  .AddNewtonsoftJson(options =>
  {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
  });

var app = builder.Build();


//Register Syncfusion license
var syncfusionLicense = Environment.GetEnvironmentVariable("Syncfusion_License");
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);

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
    "default",
    "{controller=Home}/{action=Index}/{id?}")
  .WithStaticAssets();

app.Run();