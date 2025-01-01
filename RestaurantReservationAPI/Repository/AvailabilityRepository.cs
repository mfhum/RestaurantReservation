using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly DataContext _context;
    private readonly IReservationRepository _reservationRepository;

    public AvailabilityRepository(DataContext context, IReservationRepository reservationRepository)
    {
        _context = context;
        _reservationRepository = reservationRepository;
    }

    public async Task<List<Availability>> GetAvailabilityByTimeAndGuests(DateTime reservationTime, int numberOfGuests)
    {
        // Generate 15-minute intervals for the next 2 hours
        var timeSlots = new List<DateTime> { reservationTime };
        for (var i = 1; i <= 8; i++)
        {
            timeSlots.Add(reservationTime.AddMinutes(i * 15));
        }

        // Fetch all tables and reservations for the relevant time range
        var allTables = await _context.Tables.ToListAsync();
        var reservations = await _reservationRepository.GetReservationsByTimeRange(
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
