using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IOpeningHoursRepository : IBaseRepository<OpeningHours>
{
  Task<bool> DeleteByWeekday(DayOfWeek weekday);
}