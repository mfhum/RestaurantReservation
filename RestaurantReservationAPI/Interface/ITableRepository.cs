using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface ITableRepository: IBaseRepository<Table>
{
  Task<bool> UpdateTableSeats(Guid tableId, int newSeatNumber);

}