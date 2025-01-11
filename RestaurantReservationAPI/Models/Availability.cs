namespace RestaurantReservationAPI.Models;

public class Availability
{
  public DateTime ReservationTime { get; set; }
  public int TableCount { get; set; }
}

public class AvailabilityForDay
{
  public required int Day { get; set; }
  public required int State { get; set; }
}