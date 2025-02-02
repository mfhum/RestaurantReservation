using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class OpeningHoursRepository(DataContext context) : BaseRepository<OpeningHours>(context), IOpeningHoursRepository
{
    private DataContext Context { get; } = context;

    public new async Task<OpeningHours?> CreateAsync(OpeningHours newOpeningHours)
    {
        var existingOpeningHours = await Context.OpeningHours
            .FirstOrDefaultAsync(o => o.RestaurantId == newOpeningHours.RestaurantId && o.Day == newOpeningHours.Day);

        if (existingOpeningHours != null)
        {
            return await UpdateEntityByIdAsync(newOpeningHours);
        }

        await ValidateOpeningHours(newOpeningHours);
        
        var restaurant = await Context.Restaurants.FindAsync(newOpeningHours.RestaurantId)
            ?? throw new ArgumentException("Restaurant not found");

        newOpeningHours.Restaurant = restaurant;
        await Context.OpeningHours.AddAsync(newOpeningHours);
        await Context.SaveChangesAsync();
        return newOpeningHours;
    }

    public new async Task<OpeningHours?> UpdateEntityByIdAsync(OpeningHours updateOpeningHours)
    {
        await ValidateOpeningHours(updateOpeningHours);

        var existingOpeningHours = await Context.OpeningHours
            .FirstOrDefaultAsync(o => o.RestaurantId == updateOpeningHours.RestaurantId && o.Day == updateOpeningHours.Day)
            ?? throw new ArgumentException("Opening hours not found");

        existingOpeningHours.OpeningTime = updateOpeningHours.OpeningTime;
        existingOpeningHours.BreakStartTime = updateOpeningHours.BreakStartTime;
        existingOpeningHours.BreakEndTime = updateOpeningHours.BreakEndTime;
        existingOpeningHours.ClosingTime = updateOpeningHours.ClosingTime;

        Context.OpeningHours.Update(existingOpeningHours);
        await Context.SaveChangesAsync();

        return existingOpeningHours;
    }

    // ==============================
    // Private Helper Method
    // ==============================
    
    private async Task ValidateOpeningHours(OpeningHours openingHours)
    {
        var restaurantExists = await Context.Restaurants.AnyAsync(r => r.RestaurantId == openingHours.RestaurantId);
        if (!restaurantExists)
        {
            throw new ArgumentException("Restaurant not found");
        }

        // Adjust closing time if it goes past midnight
        if (openingHours.ClosingTime < openingHours.OpeningTime)
        {
            openingHours.ClosingTime = new TimeSpan(1, openingHours.ClosingTime.Hours, openingHours.ClosingTime.Minutes, 0);
        }

        if (openingHours.OpeningTime >= openingHours.ClosingTime)
        {
            throw new ArgumentException("Opening time must be before closing time.");
        }

        if ((openingHours.BreakStartTime == null) != (openingHours.BreakEndTime == null))
        {
            throw new ArgumentException("Both BreakStartTime and BreakEndTime must be provided or both must be null.");
        }

        if (openingHours.BreakStartTime is not null && openingHours.BreakEndTime is not null)
        {
            if (openingHours.BreakStartTime >= openingHours.BreakEndTime)
            {
                throw new ArgumentException("BreakStartTime must be before BreakEndTime.");
            }

            if (openingHours.BreakStartTime < openingHours.OpeningTime)
            {
                throw new ArgumentException("BreakStartTime must be after OpeningTime.");
            }

            if (openingHours.BreakEndTime > openingHours.ClosingTime)
            {
                throw new ArgumentException("BreakEndTime must be before ClosingTime.");
            }
        }
    }
}