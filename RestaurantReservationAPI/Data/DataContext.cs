using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
  public DbSet<Reservation> Reservations { get; set; }
  public DbSet<Table> Tables { get; set; }
  public DbSet<Restaurant> Restaurants { get; set; }

  public DbSet<OpeningHours> OpeningHours { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Reservation>()
      .HasOne(r => r.Table)
      .WithMany(t => t.Reservations)
      .HasForeignKey(r => r.TableId);
    modelBuilder.Entity<OpeningHours>()
      .HasOne(o => o.Restaurant)
      .WithMany(r => r.OpeningHours)
      .HasForeignKey(o => o.RestaurantId);
  }
}