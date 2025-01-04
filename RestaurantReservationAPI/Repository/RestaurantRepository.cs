using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class RestaurantRepository(DataContext context) : BaseRepository<Restaurant>(context), IRestaurantRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> UpdateRestaurantOpeningHours(Guid restaurantId, TimeSpan newOpeningTime, TimeSpan? newBreakStartTime,
        TimeSpan? newBreakEndTime, TimeSpan newClosingTime)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null) throw new ArgumentException("Restaurant not found");
        if (newOpeningTime >= newClosingTime)
        {
            throw new ArgumentException("Opening time must be before closing time");
        }
        if (newBreakStartTime != null && newBreakEndTime != null)
        {
            if (newBreakStartTime >= newBreakEndTime)
            {
                throw new ArgumentException("Break start time must be before break end time");
            }
            if (newOpeningTime >= newBreakStartTime || newBreakEndTime >= newClosingTime)
            {
                throw new ArgumentException("Break time must be within opening hours");
            }
        }
        restaurant.OpeningTime = newOpeningTime;
        if (newBreakStartTime != null && newBreakEndTime != null)
        {
            restaurant.BreakStartTime = newBreakStartTime;
            restaurant.BreakEndTime = newBreakEndTime;
        } else {
            restaurant.BreakStartTime = null;
            restaurant.BreakEndTime = null;
        }
        restaurant.ClosingTime = newClosingTime;

        _context.Restaurants.Update(restaurant);
        await _context.SaveChangesAsync();
        return true;
    }
}