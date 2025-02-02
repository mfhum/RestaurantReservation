using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IReservationRepository: IBaseRepository<Reservation>
{
  Task<ICollection<Reservation>> GetReservationsByTimeRange(DateTime startTime, DateTime endTime);
  Task<ICollection<Reservation>> GetReservationsByTableId(Guid tableId);
  Task<ICollection<Reservation>> GetReservationsByTimeRangeAndTableSize(DateTime startTime, DateTime endTime, int numberOfGuests);
  Task<Reservation> CreateReservationByGuestNumber(Reservation reservation);

}