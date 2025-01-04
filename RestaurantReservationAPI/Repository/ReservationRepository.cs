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
    // Check if Restaurant is open
    var openingHours = await Context.OpeningHours
      .FirstOrDefaultAsync(o => o.Day == newReservation.ReservationDate.DayOfWeek);
    if (openingHours == null)
    {
      throw new ArgumentException("Restaurant is closed on this day");
    }
    // Check if Reservation is within opening hours
    var openingTime = new DateTime(newReservation.ReservationDate.Year, newReservation.ReservationDate.Month,
      newReservation.ReservationDate.Day, openingHours.OpeningTime.Hours, openingHours.OpeningTime.Minutes, 0);
    if (openingHours is { BreakStartTime: not null, BreakEndTime: not null })
    {
      var breakStartTime = new DateTime(newReservation.ReservationDate.Year, newReservation.ReservationDate.Month,
        newReservation.ReservationDate.Day, openingHours.BreakStartTime.Value.Hours, openingHours.BreakStartTime.Value.Minutes, 0);
      var breakEndTime = new DateTime(newReservation.ReservationDate.Year, newReservation.ReservationDate.Month,
        newReservation.ReservationDate.Day, openingHours.BreakEndTime.Value.Hours, openingHours.BreakEndTime.Value.Minutes, 0);
      if (newReservation.ReservationDate >= breakStartTime && newReservation.ReservationDate <= breakEndTime)
      {
        throw new ArgumentException("Reservation is during break time");
      }
    }
    // Closing time can be after Midnight
    if (openingHours.ClosingTime.Days == 1)
    {
      newReservation.ReservationDate = newReservation.ReservationDate.AddDays(1);
    }
    var closingTime = new DateTime(newReservation.ReservationDate.Year, newReservation.ReservationDate.Month,
      newReservation.ReservationDate.Day + openingHours.ClosingTime.Days, openingHours.ClosingTime.Hours, openingHours.ClosingTime.Minutes, 0);
    if (newReservation.ReservationDate < openingTime || newReservation.ReservationDate > closingTime)
    {
      throw new ArgumentException("Reservation is outside of opening hours");
    }
    await Context.Reservations.AddAsync(newReservation);
    await Context.SaveChangesAsync();
    return newReservation;
  }

  public new async Task<Reservation?> UpdateEntityByIdAsync(Reservation updateReservation)
  {
    // Validate Time
    var minutes = updateReservation.ReservationDate.Minute;
    if (minutes % 15 != 0)
    {
      throw new ArgumentException("Reservation time invalid");
    }

    // Validate Table
    var table = await Context.Tables.FindAsync(updateReservation.TableId);
    if (table == null)
    {
      throw new ArgumentException("Table not found");
    }
    // Validate Seats
    if (updateReservation.Guests > table.Seats)
    {
      throw new ArgumentException("Not enough seats");
    }
    // Only make Reservations into future
    if (updateReservation.ReservationDate < DateTime.Now)
    {
      throw new ArgumentException("Reservation date in the past");
    }
    // Check if Table is already reserved by another reservation
    var reservationEndTime = updateReservation.ReservationDate.AddMinutes(90);
    var conflictingReservations = await Context.Reservations
      .Where(r => r.TableId == updateReservation.TableId &&
                  r.ReservationDate < reservationEndTime &&
                  r.ReservationDate.AddMinutes(90) > updateReservation.ReservationDate &&
                  r.ReservationId != updateReservation.ReservationId)
      .ToListAsync();
    if (conflictingReservations.Count != 0)
    {
      throw new ArgumentException("Table is already reserved for the selected time.");
    }
    // Check if Restaurant is open
    var openingHours = await Context.OpeningHours
      .FirstOrDefaultAsync(o => o.Day == updateReservation.ReservationDate.DayOfWeek);
    if (openingHours == null)
    {
      throw new ArgumentException("Restaurant is closed on this day");
    }
    // Check if Reservation is within opening hours
    var openingTime = new DateTime(updateReservation.ReservationDate.Year, updateReservation.ReservationDate.Month,
      updateReservation.ReservationDate.Day, openingHours.OpeningTime.Hours, openingHours.OpeningTime.Minutes, 0);
    if (openingHours is { BreakStartTime: not null, BreakEndTime: not null })
    {
      var breakStartTime = new DateTime(updateReservation.ReservationDate.Year, updateReservation.ReservationDate.Month,
        updateReservation.ReservationDate.Day, openingHours.BreakStartTime.Value.Hours, openingHours.BreakStartTime.Value.Minutes, 0);
      var breakEndTime = new DateTime(updateReservation.ReservationDate.Year, updateReservation.ReservationDate.Month,
        updateReservation.ReservationDate.Day, openingHours.BreakEndTime.Value.Hours, openingHours.BreakEndTime.Value.Minutes, 0);
      if (updateReservation.ReservationDate >= breakStartTime && updateReservation.ReservationDate <= breakEndTime)
      {
        throw new ArgumentException("Reservation is during break time");
      }
    }
    // Closing time can be after Midnight
    if (openingHours.ClosingTime.Days == 1)
    {
      updateReservation.ReservationDate = updateReservation.ReservationDate.AddDays(1);
    }
    var closingTime = new DateTime(updateReservation.ReservationDate.Year, updateReservation.ReservationDate.Month,
      updateReservation.ReservationDate.Day + openingHours.ClosingTime.Days, openingHours.ClosingTime.Hours, openingHours.ClosingTime.Minutes, 0);
    if (updateReservation.ReservationDate < openingTime || updateReservation.ReservationDate > closingTime)
    {
      throw new ArgumentException("Reservation is outside of opening hours");
    }
    Context.Reservations.Update(updateReservation);
    await Context.SaveChangesAsync();
    return updateReservation;
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