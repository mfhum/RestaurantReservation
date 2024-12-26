using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class ReservationRepository(DataContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
  private DataContext Context { get; } = context;
}