using System.Diagnostics;
using System.Net.Sockets;
using FSO.API.Models;

//using FSO.API.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.EntityFrameworkCore.Extensions;


namespace FSO.API.Data;


public class ApiDbContext : DbContext
{
  private readonly IConfiguration? _configuration;
  private readonly string? connectionString;

  public DbSet<Member> Members { get; set; } = null!;
  public DbSet<Event> Events { get; set; } = null!;
  //public DbSet<SerialNumber> SerialNumbers { get; set; } = null!;

  public ApiDbContext()
  {
  } 

  public ApiDbContext(DbContextOptions<ApiDbContext> options, IConfiguration configuration) : base(options)
  {
    _configuration = configuration;

    //_connectionString = _configuration.GetValue<string>("ConnectionString");          // For local User Secrets
    //connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");      // For Enviroment Variables
    //connectionString = _configuration.GetConnectionString("CONNECTION_STRING");    
    connectionString = _configuration.GetValue<string>("ConnectionString");    
               
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    //string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING"); // For Environment Variables
    optionsBuilder
      //.EnableSensitiveDataLogging() // Remove this line in production
      .UseNpgsql(connectionString);
    

    if (string.IsNullOrEmpty(connectionString))
    {
      throw new InvalidOperationException("Connection string not found or is empty.");
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("fso");

    modelBuilder.Entity<Member>(member =>
    {       
      member.HasKey(p => p.Id)
            .HasName("PK_MemberId");

      //member.HasOne(s => s.SerialNumber)
      //      .WithOne(m => m.Member)
      //      .HasForeignKey<SerialNumber>(m => m.MemberId)
      //      .IsRequired();

       //member.Property(p => p.Id)
       // //.ValueGeneratedOnAdd()
       //     .HasConversion<UlidToStringConverter>()
       //     .HasConversion<UlidToBytesConverter>();

    });  


    //modelBuilder.Entity<SerialNumber>(serialNumber =>
    //{
    //  serialNumber.HasKey(p => new { p.Id })
    //              .HasName("PK_SerialNumberId");

    //  //serialNumber.Property(p => p.Id)
    //  //            //.ValueGeneratedOnAdd();
    //  //            //.HasConversion<UlidToStringConverter>()
    //  //            //.HasConversion<UlidToBytesConverter>();

    //  //serialNumber.Property(p => p.MemberId)
    //  //            .ValueGeneratedNever()
    //  //            .HasConversion<UlidToStringConverter>()
    //  //            .HasConversion<UlidToBytesConverter>();


    //  //serialNumber.Property(p => p.Name)
    //  //            .IsRequired()
    //  //            .HasMaxLength(100);

    //  //serialNumber.HasOne(m => m.Member)
    //  //            .WithOne(s => s.SerialNumber)
    //  //            .HasForeignKey<Member>(s => s.Id);
    //});

    modelBuilder.Entity<Event>(task =>
    {
      task.HasKey(p => p.Id)
          .HasName("PK_EventId");

      //modelBuilder.Entity<Event>()
      //.Property(task => task.Id)
      //task.ValueGeneratedOnAdd();  // Sets the primary key (PK) name

      //modelBuilder.Entity<Event>()
      //    .Property(start => start.StartDate)
      //    .HasDefaultValueSql("getdate()");

      //modelBuilder.Entity<Event>()
      //    .Property(start => start.EndDate)
      //    .HasDefaultValueSql("getdate()");


    });

   
    
  }

  public class UlidToBytesConverter : ValueConverter<Ulid, byte[]>
  {
    private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 16);

    public UlidToBytesConverter() : this(null)
    {
    }

    public UlidToBytesConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.ToByteArray(),
                convertFromProviderExpression: x => new Ulid(x),
                mappingHints: DefaultHints.With(mappingHints))
    {
    }
  }

  public class UlidToStringConverter : ValueConverter<Ulid, string>
  {
    private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 26);

    public UlidToStringConverter() : this(null)
    {
    }

    public UlidToStringConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.ToString(),
                convertFromProviderExpression: x => Ulid.Parse(x),
                mappingHints: DefaultHints.With(mappingHints))
    {
    }
  }

}
