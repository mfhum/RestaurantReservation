using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IRestaurantRepository : IBaseRepository<Restaurant>
{
  Task<bool> UpdateRestaurantOpeningHours(Guid restaurantId, TimeSpan newOpeningTime, TimeSpan? newBreakStartTime,
    TimeSpan? newBreakEndTime, TimeSpan newClosingTime);
}