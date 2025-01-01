using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IAvailabilityRepository
{
  Task<ICollection<Availability>> GetAvailabilityByTimeAndGuests(DateTime reservationTime, int numberOfGuests);
}