using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class ReservationRepository(DataContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
  private DataContext Context { get; } = context;

  public new async Task<Reservation?> CreateAsync(Reservation newReservation)
  {
    // Validate Time
    var minutes = newReservation.ReservationDate.Minute;
    if (minutes % 15 != 0)
    {
      throw new ArgumentException("Reservation time invalid");
    }

    // Validate Table
    var table = await Context.Tables.FindAsync(newReservation.TableId);
    if (table == null)
    {
      throw new ArgumentException("Table not found");
    }
    // Validate Seats
    if (newReservation.Guests > table.Seats)
    {
      throw new ArgumentException("Not enough seats");
    }
    // Only make Reservations into future
    if (newReservation.ReservationDate < DateTime.Now)
    {
      throw new ArgumentException("Reservation date in the past");
    }
    // Check if Table is already reserved
    var reservationEndTime = newReservation.ReservationDate.AddMinutes(90);
    var conflictingReservations = await Context.Reservations
      .Where(r => r.TableId == newReservation.TableId &&
                  r.ReservationDate < reservationEndTime &&
                  r.ReservationDate.AddMinutes(90) > newReservation.ReservationDate)
      .ToListAsync();
    if (conflictingReservations.Count != 0)
    {
      throw new ArgumentException("Table is already reserved for the selected time.");
    }
    await Context.Reservations.AddAsync(newReservation);
    await Context.SaveChangesAsync();
    return newReservation;
  }

  public async Task<ICollection<Reservation>> GetReservationsByTimeRange(DateTime startTime, DateTime endTime)
  {
    var allReservations = await GetAllAsync();
    var reservations = allReservations.Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime).ToList();
    return reservations;
  }

  public async Task<ICollection<Reservation>> GetReservationsByTableId(Guid tableId)
  {
    var reservations = await Context.Reservations.Where(r => r.TableId == tableId).ToListAsync();
    return reservations;
  }


}