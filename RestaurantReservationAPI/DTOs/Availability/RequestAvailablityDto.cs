namespace RestaurantReservationAPI.DTOs.Availability;

public class RequestAvailablityDto
{
  public required DateTime ReservationTime { get; set; }
  public required int NumberOfGuests { get; set; }
}

public class RequestGetGeneralAvailabilityDto : RequestAvailablityDto;

public class RequestGetAvailabilityForMonthDto : RequestAvailablityDto;