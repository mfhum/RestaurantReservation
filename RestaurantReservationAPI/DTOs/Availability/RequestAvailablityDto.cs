namespace RestaurantReservationAPI.DTOs.Availability;

public class RequestAvailablityDto
{

}

public class RequestGetGeneralAvailabilityDto
{
  public required DateTime ReservationTime { get; set; }
  public required int NumberOfGuests { get; set; }
}