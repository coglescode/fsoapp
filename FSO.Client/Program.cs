using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using FSO.Client.Services;
using FSO.Client.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<MembersApiService>();
//builder.Services.AddHttpClient<EventsApiService>();

//builder.Services.AddDbContext<ClientDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings:DefaultConnection")));
builder.Configuration.AddEnvironmentVariables(prefix: "ApiEndpointUrl_");


// builder.WebHost.ConfigureKestrel((context, serverOptions) =>
// {
//     var kestrelSection = context.Configuration.GetSection("Kestrel");

//     serverOptions.Configure(kestrelSection)
//         .Endpoint("ApiEndpoint", listenOptions =>
//     {

//     });

// });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{    

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
