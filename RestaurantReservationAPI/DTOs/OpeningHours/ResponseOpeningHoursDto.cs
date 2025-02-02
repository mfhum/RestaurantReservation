namespace RestaurantReservationAPI.DTOs.OpeningHours;

public class ResponseOpeningHoursDto
{
  public required Guid OpeningHoursId { get; set; }
  public required DayOfWeek Day { get; set; }
  public required TimeSpan OpeningTime { get; set; }
  public TimeSpan? BreakStartTime { get; set; }
  public TimeSpan? BreakEndTime { get; set; }
  public required TimeSpan ClosingTime { get; set; }
}

public class ResponseGetAllOpeningHoursDto : ResponseOpeningHoursDto;

public class ResponseGetOpeningHoursDto : ResponseOpeningHoursDto;

public class ResponseCreateOpeningHoursDto : ResponseOpeningHoursDto;

public class ResponseUpdateOpeningHoursDto : ResponseOpeningHoursDto;

public class ResponseDeleteOpeningHoursDto
{
  public required Guid OpeningHoursId { get; set; }
}