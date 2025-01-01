using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class AvailabilityRepository(DataContext context, IReservationRepository reservationRepository)
    : IAvailabilityRepository
{
    public async Task<ICollection<Availability>> GetAvailabilityByTimeAndGuests(DateTime reservationTime, int numberOfGuests)
    {
        var roundedReservationTime = new DateTime(
            reservationTime.Year,
            reservationTime.Month,
            reservationTime.Day,
            reservationTime.Hour,
            reservationTime.Minute % 15 == 0 ? reservationTime.Minute : (reservationTime.Minute % 15 + 1) * 15 % 60,
            0,
            000,
            DateTimeKind.Utc);
        // Generate 15-minute intervals for the next 2 hours
        var timeSlots = new List<DateTime> { roundedReservationTime };
        for (var i = 1; i <= 8; i++)
        {
            timeSlots.Add(roundedReservationTime.AddMinutes(i * 15));
        }

        // Fetch all tables and reservations for the relevant time range
        var allTables = await context.Tables.ToListAsync();
        var reservations = await reservationRepository.GetReservationsByTimeRange(
            timeSlots.First(),
            timeSlots.Last().AddMinutes(90) // Include overlapping reservations
        );

        // Calculate availability for each time slot
        var availability = (from timeSlot in timeSlots
            let reservedTableIds = reservations
                .Where(r => r.ReservationDate <= timeSlot.AddMinutes(90) && r.ReservationDate.AddMinutes(90) > timeSlot)
                .Select(r => r.TableId)
                .ToHashSet()
            let freeTablesCount = allTables
                .Count(t => t.Seats >= numberOfGuests && !reservedTableIds.Contains(t.TableId))
            select new Availability
            {
                ReservationTime = timeSlot,
                TableCount = freeTablesCount // Count of free tables
            }).ToList();

        return availability;
    }
}
