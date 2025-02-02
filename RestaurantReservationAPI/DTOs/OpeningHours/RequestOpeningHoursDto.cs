using RestaurantReservationAPI.DTOs.Generic;

namespace RestaurantReservationAPI.DTOs.OpeningHours;

public class RequestOpeningHoursDto
{
  public required DayOfWeek Day { get; set; }
  public required TimeSpan OpeningTime { get; set; }
  public TimeSpan? BreakStartTime { get; set; }
  public TimeSpan? BreakEndTime { get; set; }
  public required TimeSpan ClosingTime { get; set; }
}

public class RequestCreateOpeningHoursDto : RequestOpeningHoursDto;

public class RequestUpdateOpeningHoursDto : RequestOpeningHoursDto
{
  public required Guid OpeningHoursId { get; set; }
};

public class RequestDeleteOpeningHoursDto : Generics.IHasId
{
  public Guid Id { get; set; }
}

public class RequestGetOpeningHoursDto
{
  public required Guid OpeningHoursId { get; set; }
}

public class RequestGetAllOpeningHoursDto;