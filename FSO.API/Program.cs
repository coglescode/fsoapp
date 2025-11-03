using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using FSO.API.Models;
using Microsoft.Extensions.Options;
using System;
using FSO.API.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using FSO.API.Data;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


// Add services to the container.
//var testString = builder.Configuration.AddEnvironmentVariables(prefix: "CONNECTION_STRING_"); // You need to check the usage of this  line
builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("POSTGRES_STRING");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddDbContext<ApiDbContext>(options =>
////    options.UseSqlServer("Members"));
//    options.UseSqlServer(connectionString));



//string? connectstring = builder.Configuration["CONNECTION_STRING_"];
//builder.Services.AddAwsSecretsManager(builder.Configuration);

//var env = builder.Environment.EnvironmentName;
//var appName = builder.Environment.ApplicationName;


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapEventEndpoints();

app.Run();
