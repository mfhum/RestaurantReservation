using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IReservationRepository: IBaseRepository<Reservation>
{
  Task<List<Reservation>> GetReservationsByTimeRange(DateTime startTime, DateTime endTime);


}