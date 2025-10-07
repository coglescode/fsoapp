using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using FSO.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FSO.App.Data;

public class FSOAppContext : DbContext
{
  private readonly IConfiguration _configuration;
  private readonly string? connectionString;

  public DbSet<Member> Members { get; set; } = default!;
  public DbSet<Event> Events { get; set; } = default!; // Uncomment if you have an Event model

  public DbSet<SerialNumber> SerialNumbers { get; set; } = default!;


  public FSOAppContext (DbContextOptions<FSOAppContext> options, IConfiguration configuration) : base(options)
  {
      _configuration = configuration;

      connectionString = _configuration.GetValue<string>("ConnectionString");
      //connectionString = _configuration.GetConnectionString("POSTGRES_STRING");
  }
     

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {

    //connectionString = Environment.GetEnvironmentVariable("POSTGRES_STRING"); // For Environment Variables
    optionsBuilder.UseNpgsql(connectionString);
    

    //if (string.IsNullOrEmpty(connectionString))
    //{
    //  throw new InvalidOperationException("Connection string 'POSTGRES_STRING' not found or is empty.");
    //}

  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("fso");

    modelBuilder.Entity<Member>(member =>
    {
      member.HasKey(p => p.Id).HasName("PK_MemberId");

      //member.HasOne(s => s.SerialNumber)
      //      .WithOne(m => m.Member)
      //      .HasForeignKey<SerialNumber>(m => m.MemberId)
      //      .IsRequired();

      //member.Property(p => p.Id)
            //.ValueGeneratedOnAdd()
            //.HasConversion<UlidToStringConverter>();
            //.HasConversion<UlidToBytesConverter>();

    });

    modelBuilder.Entity<Event>(task =>
    {
      task.HasKey(p => p.Id)
          .HasName("PK_EventId");

      //member.HasOne(s => s.SerialNumber)
      //      .WithOne(m => m.Member)
      //      .HasForeignKey<SerialNumber>(m => m.MemberId)
      //      .IsRequired();

      task.Property(p => p.StartDate)
          .HasColumnType("date");
          //.HasDefaultValueSql("getdate()");
      
      task.Property(p => p.EndDate)
          .HasColumnType("date"); // Modified to use date type


    });

    modelBuilder.Entity<SerialNumber>(serialNumber =>
    {
      serialNumber.HasKey(p => new { p.Id })
                  .HasName("PK_SerialNumberId");

      serialNumber.Property(p => p.Id)
                  .ValueGeneratedNever();

      //serialNumber.Property(p => p.Name)
      //            .IsRequired()
      //            .HasMaxLength(100);

      //serialNumber.HasOne(m => m.Member)
      //            .WithOne(s => s.SerialNumber)
      //            .HasForeignKey<Member>(s => s.Id);
    });

    // Task Entity
    //
    //modelBuilder.Entity<Event>()
    //.HasKey(task => new { task.Id })
    //.HasName("EventId");  // Sets the primary key (PK) name

    //modelBuilder.Entity<Event>()
    //.Property(task => task.Id)
    //.ValueGeneratedOnAdd();  // Sets the primary key (PK) name

    //modelBuilder.Entity<Event>()
    //    .Property(start => start.StartDate)
    //    .HasDefaultValueSql("getdate()");

    //modelBuilder.Entity<Event>()
    //    .Property(start => start.EndDate)
    //    .HasDefaultValueSql("getdate()");
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
