using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Helpers.Mail;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class ReservationRepository(DataContext context, IEmailService emailService) : BaseRepository<Reservation>(context), IReservationRepository
{
    private DataContext Context { get; } = context;

    public new async Task<Reservation?> CreateAsync(Reservation newReservation)
    {
        newReservation.ReservationDate = EnsureUtc(newReservation.ReservationDate);
        await ValidateReservation(newReservation);
        await Context.Reservations.AddAsync(newReservation);
        await Context.SaveChangesAsync();
        return newReservation;
    }

    public new async Task<Reservation?> UpdateEntityByIdAsync(Reservation updateReservation)
    {
        updateReservation.ReservationDate = EnsureUtc(updateReservation.ReservationDate);
        await ValidateReservation(updateReservation, updateReservation.ReservationId);
        Context.Reservations.Update(updateReservation);
        await Context.SaveChangesAsync();
        return updateReservation;
    }

    public async Task<Reservation> CreateReservationByGuestNumber(Reservation newReservation)
    {
        newReservation.ReservationDate = EnsureUtc(newReservation.ReservationDate);
        await ValidateReservationTimeAndOpeningHours(newReservation);
        
        var availableTables = await Context.Tables
            .Where(t => t.Seats >= newReservation.Guests && t.Seats <= newReservation.Guests + 2)
            .ToListAsync();

        if (!availableTables.Any()) 
            throw new ArgumentException("No available table for the amount of guests");

        var reservationEndTime = newReservation.ReservationDate.AddMinutes(90);

        foreach (var table in availableTables)
        {
            var isTableAvailable = !await Context.Reservations.AnyAsync(r => 
                r.TableId == table.TableId &&
                r.ReservationDate < reservationEndTime &&
                r.ReservationDate.AddMinutes(90) > newReservation.ReservationDate);

            if (isTableAvailable)
            {
                newReservation.TableId = table.TableId;
                break;
            }
        }

        if (newReservation.TableId == Guid.Empty)
            throw new ArgumentException("Table is already reserved for the selected time.");

        await Context.Reservations.AddAsync(newReservation);
        await Context.SaveChangesAsync();

        // Send email confirmation
        if (!string.IsNullOrEmpty(newReservation.Mail))
        {
            var reservationDetails = $"Date: {newReservation.ReservationDate:dd MMM yyyy}\n" +
                                     $"Time: {newReservation.ReservationDate:HH:mm}\n" +
                                     $"Guests: {newReservation.Guests}";

            await emailService.SendReservationConfirmationAsync(newReservation.Mail, reservationDetails);
        }

        return newReservation;
    }

    public async Task<ICollection<Reservation>> GetReservationsByTimeRange(DateTime startTime, DateTime endTime)
    {
        startTime = EnsureUtc(startTime);
        endTime = EnsureUtc(endTime);

        return await Context.Reservations
            .Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime)
            .ToListAsync();
    }

    public async Task<ICollection<Reservation>> GetReservationsByTableId(Guid tableId)
    {
        return await Context.Reservations
            .Where(r => r.TableId == tableId)
            .ToListAsync();
    }

    public async Task<ICollection<Reservation>> GetReservationsByTimeRangeAndTableSize(DateTime startTime, DateTime endTime, int numberOfGuests)
    {
        startTime = EnsureUtc(startTime);
        endTime = EnsureUtc(endTime);

        return await Context.Reservations
            .Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime && r.Guests == numberOfGuests)
            .ToListAsync();
    }

    // ==============================
    // Private Helper Methods
    // ==============================

    private async Task ValidateReservation(Reservation reservation, Guid? updatingReservationId = null)
    {
        reservation.ReservationDate = EnsureUtc(reservation.ReservationDate);
        await ValidateReservationTimeAndOpeningHours(reservation);

        var table = await Context.Tables.FindAsync(reservation.TableId)
            ?? throw new ArgumentException("Table not found");

        if (reservation.Guests > table.Seats)
            throw new ArgumentException("Not enough seats");

        var reservationEndTime = reservation.ReservationDate.AddMinutes(90);

        var isTableReserved = await Context.Reservations.AnyAsync(r => 
            r.TableId == reservation.TableId &&
            r.ReservationDate < reservationEndTime &&
            r.ReservationDate.AddMinutes(90) > reservation.ReservationDate &&
            r.ReservationId != updatingReservationId);

        if (isTableReserved)
            throw new ArgumentException("Table is already reserved for the selected time.");
    }

    private async Task ValidateReservationTimeAndOpeningHours(Reservation reservation)
    {
        reservation.ReservationDate = EnsureUtc(reservation.ReservationDate);

        if (reservation.ReservationDate < DateTime.UtcNow)
            throw new ArgumentException("Reservation date in the past");

        if (reservation.ReservationDate.Minute % 15 != 0)
            throw new ArgumentException("Reservation time invalid");

        var openingHours = await Context.OpeningHours
            .FirstOrDefaultAsync(o => o.Day == reservation.ReservationDate.DayOfWeek)
            ?? throw new ArgumentException("Restaurant is closed on this day");

        var openingTime = EnsureUtc(new DateTime(
            reservation.ReservationDate.Year, reservation.ReservationDate.Month, reservation.ReservationDate.Day, 
            openingHours.OpeningTime.Hours, openingHours.OpeningTime.Minutes, 0));

        var closingTime = EnsureUtc(new DateTime(
            reservation.ReservationDate.Year, reservation.ReservationDate.Month, reservation.ReservationDate.Day,
            openingHours.ClosingTime.Hours, openingHours.ClosingTime.Minutes, 0));

        if (openingHours.ClosingTime.Days == 1)
            closingTime = closingTime.AddDays(1);

        if (openingHours.BreakStartTime != null && openingHours.BreakEndTime != null)
        {
            var breakStartTime = EnsureUtc(new DateTime(
                reservation.ReservationDate.Year, reservation.ReservationDate.Month, reservation.ReservationDate.Day,
                openingHours.BreakStartTime.Value.Hours, openingHours.BreakStartTime.Value.Minutes, 0));

            var breakEndTime = EnsureUtc(new DateTime(
                reservation.ReservationDate.Year, reservation.ReservationDate.Month, reservation.ReservationDate.Day,
                openingHours.BreakEndTime.Value.Hours, openingHours.BreakEndTime.Value.Minutes, 0));

            if (reservation.ReservationDate >= breakStartTime && reservation.ReservationDate <= breakEndTime)
                throw new ArgumentException("Reservation is during break time");
        }

        if (reservation.ReservationDate < openingTime || reservation.ReservationDate > closingTime)
            throw new ArgumentException("Reservation is outside of opening hours");
    }

    private static DateTime EnsureUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) : dateTime.ToUniversalTime();
    }
}