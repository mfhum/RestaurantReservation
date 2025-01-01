using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IReservationRepository: IBaseRepository<Reservation>
{
  Task<ICollection<Reservation>> GetReservationsByTimeRange(DateTime startTime, DateTime endTime);


}