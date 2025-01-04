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
      throw new ArgumentException("Opening hours for this day already exists");
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
    // check if opening hours for this restaurant already exists for that day on other opening hours
    var openingHours = await Context.OpeningHours
      .FirstOrDefaultAsync(o => o.RestaurantId == updateOpeningHours.RestaurantId && o.Day == updateOpeningHours.Day && o.OpeningHoursId != updateOpeningHours.OpeningHoursId);
    if (openingHours != null)
    {
      throw new ArgumentException("Opening hours for this day already exists");
    }
    // check if closing time is after midnight, if yes add Day + 1
    if (updateOpeningHours.ClosingTime < updateOpeningHours.OpeningTime)
    {
      updateOpeningHours.ClosingTime =
        new TimeSpan(1, updateOpeningHours.ClosingTime.Hours, updateOpeningHours.ClosingTime.Minutes, 0);
    }
    // check if opening time is before closing time
    if (updateOpeningHours.OpeningTime >= updateOpeningHours.ClosingTime)
    {
      throw new ArgumentException("Opening time must be before closing time");
    }
    // check if there is a breakStartTime and breakEndTime if there is one of them
    if (updateOpeningHours is { BreakStartTime: not null, BreakEndTime: not null })
    {
      // check if breakStartTime is before breakEndTime
      if (updateOpeningHours.BreakStartTime >= updateOpeningHours.BreakEndTime)
      {
        throw new ArgumentException("BreakStartTime must be before BreakEndTime");
      }
      // check if breakStartTime is after openingTime
      if (updateOpeningHours.BreakStartTime < updateOpeningHours.OpeningTime)
      {
        throw new ArgumentException("BreakStartTime must be after OpeningTime");
      }
      // check if breakEndTime is before closingTime
      if (updateOpeningHours.BreakEndTime > updateOpeningHours.ClosingTime)
      {
        throw new ArgumentException("BreakEndTime must be before ClosingTime");
      }
    }
    // check if breakStartTime and breakEndTime are both null
    if (updateOpeningHours.BreakStartTime == null && updateOpeningHours.BreakEndTime != null ||
        updateOpeningHours.BreakStartTime != null && updateOpeningHours.BreakEndTime == null)
    {
      throw new ArgumentException("BreakStartTime and BreakEndTime must be both null or both not null");
    }

    //check if restaurant is valid
    var restaurant = await Context.Restaurants.FindAsync(updateOpeningHours.RestaurantId);
    if (restaurant == null)
    {
      throw new ArgumentException("Restaurant not found");
    }
    updateOpeningHours.Restaurant = restaurant;
    Context.OpeningHours.Update(updateOpeningHours);
    await Context.SaveChangesAsync();
    return updateOpeningHours;
  }
}