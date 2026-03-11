using FsoMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FsoMVC.Data;

public class FsoAppContext(DbContextOptions options) : DbContext(options)
{
  //private readonly IConfiguration _configuration;
  //private readonly string? _connectionString;

  public DbSet<Member> Members { get; set; }
  public DbSet<Event> Events { get; set; } // Uncomment if you have an Event model
  public DbSet<SerialNumber> SerialNumbers { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("fso");

    modelBuilder.Entity<Member>(member =>
    {
      member.HasKey(p => p.Id).HasName("PK_MemberId");
      
      member.HasMany(e => e.Events)
            .WithOne(m => m.MemberRelated)
            .HasForeignKey(e => e.MemberId)
            .IsRequired();
      
      member.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(100);
    });

    modelBuilder.Entity<Event>(e =>
    {
      e.HasKey(p => p.Id)
       .HasName("PK_EventId");

      //member.HasOne(s => s.SerialNumber)
      //      .WithOne(m => m.Member)
      //      .HasForeignKey<SerialNumber>(m => m.MemberId)
      //      .IsRequired();

      e.Property(p => p.StartDate)
       .HasColumnType("date");
      
      e.Property(p => p.EndDate)
       .HasColumnType("date"); // Modified to use date type
      
      // e.Property(n => n.Title)
      //   .IsRequired()
      //   .HasMaxLength(100);
    });

    // modelBuilder.Entity<SerialNumber>(serialNumber =>
    // {
    //   serialNumber.HasKey(p => new { p.Id })
    //               .HasName("PK_SerialNumberId");
    //
    //   serialNumber.Property(p => p.Id)
    //               .ValueGeneratedNever();
    //
    //   //serialNumber.Property(p => p.Name)
    //   //            .IsRequired()
    //   //            .HasMaxLength(100);
    //
    //   //serialNumber.HasOne(m => m.Member)
    //   //            .WithOne(s => s.SerialNumber)
    //   //            .HasForeignKey<Member>(s => s.Id);
    // });

    
  }
}