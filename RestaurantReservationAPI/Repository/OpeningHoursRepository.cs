using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class OpeningHoursRepository(DataContext context)
  : BaseRepository<OpeningHours>(context), IOpeningHoursRepository
{
  private DataContext Context { get; } = context;

  public new async Task<OpeningHours?> CreateAsync(OpeningHours newOpeningHours)
  {
    // check if opening hours for this restaurant already exists for that day
    var openingHours = await Context.OpeningHours
      .FirstOrDefaultAsync(o => o.RestaurantId == newOpeningHours.RestaurantId && o.Day == newOpeningHours.Day);
    if (openingHours != null)
    {
      return await UpdateEntityByIdAsync(newOpeningHours);
    }
    // check if closing time is after midnight, if yes add Day + 1
    if (newOpeningHours.ClosingTime < newOpeningHours.OpeningTime)
    {
      newOpeningHours.ClosingTime =
        new TimeSpan(1, newOpeningHours.ClosingTime.Hours, newOpeningHours.ClosingTime.Minutes, 0);
    }
    // check if opening time is before closing time
    if (newOpeningHours.OpeningTime >= newOpeningHours.ClosingTime)
    {
      throw new ArgumentException("Opening time must be before closing time");
    }
    // check if there is a breakStartTime and breakEndTime if there is one of them
    if (newOpeningHours is { BreakStartTime: not null, BreakEndTime: not null })
    {
      // check if breakStartTime is before breakEndTime
      if (newOpeningHours.BreakStartTime >= newOpeningHours.BreakEndTime)
      {
        throw new ArgumentException("BreakStartTime must be before BreakEndTime");
      }
      // check if breakStartTime is after openingTime
      if (newOpeningHours.BreakStartTime < newOpeningHours.OpeningTime)
      {
        throw new ArgumentException("BreakStartTime must be after OpeningTime");
      }
      // check if breakEndTime is before closingTime
      if (newOpeningHours.BreakEndTime > newOpeningHours.ClosingTime)
      {
        throw new ArgumentException("BreakEndTime must be before ClosingTime");
      }
    }
    // check if breakStartTime and breakEndTime are both null
    if (newOpeningHours.BreakStartTime == null && newOpeningHours.BreakEndTime != null ||
        newOpeningHours.BreakStartTime != null && newOpeningHours.BreakEndTime == null)
    {
      throw new ArgumentException("BreakStartTime and BreakEndTime must be both null or both not null");
    }

    //check if restaurant is valid
    var restaurant = await Context.Restaurants.FindAsync(newOpeningHours.RestaurantId);
    if (restaurant == null)
    {
      throw new ArgumentException("Restaurant not found");
    }
    newOpeningHours.Restaurant = restaurant;
    await Context.OpeningHours.AddAsync(newOpeningHours);
    await Context.SaveChangesAsync();
    return newOpeningHours;
  }

  public new async Task<OpeningHours?> UpdateEntityByIdAsync(OpeningHours updateOpeningHours)
{
    // Ensure restaurant exists
    var restaurant = await Context.Restaurants.FindAsync(updateOpeningHours.RestaurantId);
    if (restaurant == null)
    {
      throw new ArgumentException("Restaurant not found");
    }

    // Ensure closing time is after opening time, adjusting for midnight
    if (updateOpeningHours.ClosingTime < updateOpeningHours.OpeningTime)
    {
        updateOpeningHours.ClosingTime =
            new TimeSpan(updateOpeningHours.ClosingTime.Days + 1, updateOpeningHours.ClosingTime.Hours, updateOpeningHours.ClosingTime.Minutes, 0);
    }

    // Validate break times, if provided
    if (updateOpeningHours.BreakStartTime != null || updateOpeningHours.BreakEndTime != null)
    {
        if (updateOpeningHours.BreakStartTime == null || updateOpeningHours.BreakEndTime == null)
        {
            throw new ArgumentException("Both BreakStartTime and BreakEndTime must be provided.");
        }

        if (updateOpeningHours.BreakStartTime >= updateOpeningHours.BreakEndTime)
        {
            throw new ArgumentException("BreakStartTime must be before BreakEndTime.");
        }

        if (updateOpeningHours.BreakStartTime < updateOpeningHours.OpeningTime)
        {
            throw new ArgumentException("BreakStartTime must be after OpeningTime.");
        }

        if (updateOpeningHours.BreakEndTime > updateOpeningHours.ClosingTime)
        {
            throw new ArgumentException("BreakEndTime must be before ClosingTime.");
        }
    }

    //find the opening hours entity for the restaurant and day
    var existingOpeningHours = await Context.OpeningHours
        .FirstOrDefaultAsync(o => o.RestaurantId == updateOpeningHours.RestaurantId && o.Day == updateOpeningHours.Day);
    if (existingOpeningHours == null)
    {
        throw new ArgumentException("Opening hours not found");
    }
    //Update existingOpeningHours
    existingOpeningHours.OpeningTime = updateOpeningHours.OpeningTime;
    if (updateOpeningHours.BreakStartTime == null || updateOpeningHours.BreakEndTime == null)
    {
        existingOpeningHours.BreakStartTime = null;
        existingOpeningHours.BreakEndTime = null;
    }
    else
    {
        existingOpeningHours.BreakStartTime = updateOpeningHours.BreakStartTime;
        existingOpeningHours.BreakEndTime = updateOpeningHours.BreakEndTime;
    }
    existingOpeningHours.ClosingTime = updateOpeningHours.ClosingTime;

    Context.OpeningHours.Update(existingOpeningHours);
    await Context.SaveChangesAsync();

    return updateOpeningHours;
}

}