namespace RestaurantReservationAPI.DTOs.Availability;

public class ResponseAvailabilityDto
{

}

public class ResponseGetGeneralAvailabilityDto
{
  public required DateTime ReservationTime { get; set; }
  public required int TableCount { get; set; }
}