namespace RestaurantReservationAPI.DTOs.Availability;

public class ResponseAvailabilityDto
{

}

public class ResponseGetGeneralAvailabilityDto
{
  public required DateTime Time { get; set; }
  public required int TableCount { get; set; }
}