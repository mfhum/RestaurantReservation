using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Interface;

public interface IAvailabilityRepository
{
  Task<ICollection<Availability>> GetAvailabilityByTimeAndGuests(DateTime reservationTime, int numberOfGuests);
  Task<ICollection<AvailabilityForDay>> GetAvailabilityByMonth(DateTime reservationTime, int numberOfGuests);
  Task<bool> HasAvailability(DateTime reservationTime, int numberOfGuests);
}