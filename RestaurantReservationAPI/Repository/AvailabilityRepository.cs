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
        var roundedReservationTime = ParseTime(reservationTime, reservationTime.TimeOfDay);

        // Generate 15-minute intervals for the opening hours on that day
        var openingHours = await context.OpeningHours.FirstOrDefaultAsync(o => o.Day == reservationTime.DayOfWeek);
        if (openingHours == null) return new List<Availability>();

         var timeSlots = GenerateTimeSlots(
            ParseTime(reservationTime, openingHours.OpeningTime),
            ParseTime(reservationTime, openingHours.ClosingTime),
            openingHours.BreakStartTime.HasValue ? ParseTime(reservationTime, openingHours.BreakStartTime.Value) : null,
            openingHours.BreakEndTime.HasValue ? ParseTime(reservationTime, openingHours.BreakEndTime.Value) : null
        );


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

    public async Task<ICollection<AvailabilityForDay>> GetAvailabilityByMonth(DateTime reservationTime, int numberOfGuests)
    {
        ICollection<AvailabilityForDay> availabilityForMonth = new List<AvailabilityForDay>();
        // Get the first day of the month that was sent
        var firstDayOfMonth = new DateTime(reservationTime.Year, reservationTime.Month, 1);
        // Get the last day of the month that was sent
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        // check if month is not this month or the future
        if (firstDayOfMonth.Month < DateTime.UtcNow.AddMinutes(60).Month)
            throw new ArgumentException("Month must be in the future");
        // check if month is not more than 3 months in the future
        if (firstDayOfMonth.Month > DateTime.UtcNow.AddMonths(3).AddMinutes(60).Month)
            throw new ArgumentException("Month must be in the next 3 months");
        // get all reservations for the sent month
        var allReservationsByTimeRange = await reservationRepository.GetReservationsByTimeRangeAndTableSize(firstDayOfMonth, lastDayOfMonth, numberOfGuests);
        // loop over all
        for (var dayOfMonth = 1; dayOfMonth <= DateTime.DaysInMonth(reservationTime.Year, reservationTime.Month); dayOfMonth++)
        {
            // check if day is in the past
            if (new DateTime(reservationTime.Year, reservationTime.Month, dayOfMonth) < DateTime.UtcNow.AddMinutes(60).Date)
            {
                availabilityForMonth.Add(new AvailabilityForDay()
                {
                    Day = dayOfMonth,
                    State = 0
                });
                continue;
            }
            // check if restaurant is closed on that day
            var openingHours = await context.OpeningHours.FirstOrDefaultAsync(o =>
                o.RestaurantId == new Guid("019431d9-d9f5-7463-b20d-a3e9d6badfe0") && o.Day == (DayOfWeek)new DateTime(reservationTime.Year, reservationTime.Month, dayOfMonth).DayOfWeek);
            if (openingHours == null)
            {
                availabilityForMonth.Add(new AvailabilityForDay()
                {
                    Day = dayOfMonth,
                    State = 4
                });
                continue;
            }
            // check if day is today
            if (new DateTime(reservationTime.Year, reservationTime.Month, dayOfMonth) == DateTime.UtcNow.AddMinutes(60).Date)
            {
                availabilityForMonth.Add(new AvailabilityForDay()
                {
                    Day = dayOfMonth,
                    State = 1
                });
                continue;
            }
            // check if there is a free spot in the restaurant for that day
            var hasAvailability = await HasAvailability(new DateTime(reservationTime.Year, reservationTime.Month, dayOfMonth), numberOfGuests);
            if (hasAvailability)
            {
                availabilityForMonth.Add(new AvailabilityForDay()
                {
                    Day = dayOfMonth,
                    State = 2
                });
                continue;
            }
            availabilityForMonth.Add(new AvailabilityForDay()
            {
                Day = dayOfMonth,
                State = 3
            });
        }
        return availabilityForMonth;
    }

    public async Task<bool> HasAvailability(DateTime reservationTime, int numberOfGuests)
{
    // Get Opening Hours for the day
    var openingHours = await context.OpeningHours.FirstOrDefaultAsync(o => o.Day == reservationTime.DayOfWeek);

    // Check if the restaurant is closed
    if (openingHours == null) return false;

    // Parse opening and closing times
    var openDateTime = ParseTime(reservationTime, openingHours.OpeningTime);
    var closingDateTime = ParseTime(reservationTime, openingHours.ClosingTime);

    // Adjust for overnight closing times
    if (openingHours.ClosingTime.Days == 1) closingDateTime = closingDateTime.AddDays(1);

    // Add break handling
    DateTime? breakStartDateTime = null;
    DateTime? breakEndDateTime = null;

    if (openingHours.BreakStartTime.HasValue && openingHours.BreakEndTime.HasValue)
    {
        breakStartDateTime = ParseTime(reservationTime, openingHours.BreakStartTime.Value);
        breakEndDateTime = ParseTime(reservationTime, openingHours.BreakEndTime. Value);
    }

    // Generate 15-minute intervals excluding breaks
    var timeSlots =GenerateTimeSlots(openDateTime, closingDateTime, breakStartDateTime, breakEndDateTime);

    // Fetch all tables and reservations for the relevant time range
    var allTables = await context.Tables.ToListAsync();
    var reservations = await reservationRepository.GetReservationsByTimeRange(
        timeSlots.First(),
        timeSlots.Last().AddMinutes(90));

    // Check availability for each time slot
    return timeSlots.Select(timeSlot => reservations.Where(r => r.ReservationDate <= timeSlot.AddMinutes(90) && r.ReservationDate.AddMinutes(90) > timeSlot)
            .Select(r => r.TableId)
            .ToHashSet())
        .Select(reservedTableIds => allTables.Count(t => t.Seats >= numberOfGuests && !reservedTableIds.Contains(t.TableId)))
        .Any(freeTablesCount => freeTablesCount > 0);
}

    private static List<DateTime> GenerateTimeSlots(DateTime openDateTime, DateTime closingDateTime, DateTime? breakStartDateTime, DateTime? breakEndDateTime)
    {
        var timeSlots = new List<DateTime>();
        var currentTime = openDateTime;

        while (currentTime <= closingDateTime.AddMinutes(-60))
        {
            // Skip time slots during the break
            if (breakStartDateTime.HasValue && breakEndDateTime.HasValue &&
                currentTime >= breakStartDateTime.Value.AddMinutes(-60) && currentTime < breakEndDateTime)
            {
                currentTime = breakEndDateTime.Value;
                continue;
            }

            // Add valid time slot
            timeSlots.Add(currentTime);
            currentTime = currentTime.AddMinutes(15);
        }

        return timeSlots;
    }


    private static DateTime ParseTime(DateTime date, TimeSpan time)
    {
        return new DateTime(date.Year, date.Month, date.Day, time.Hours,
            time.Minutes % 15 == 0 ? time.Minutes : (time.Minutes / 15 + 1) * 15 % 60, 0);
    }
}