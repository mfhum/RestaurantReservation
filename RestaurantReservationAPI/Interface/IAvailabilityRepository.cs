using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IAvailabilityRepository
{
  Task<List<Availability>> GetAvailabilityByTimeAndGuests(DateTime reservationTime, int numberOfGuests);
}