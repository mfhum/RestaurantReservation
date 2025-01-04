namespace RestaurantReservationAPI.DTOs.Tables;

public class ResponseRestaurantDto
{
  public required Guid RestaurantId { get; set; }
  public required string Name { get; set; }
  public required TimeSpan OpeningTime { get; set; } // Opening time (e.g., 10:00 AM)
  public TimeSpan BreakStartTime { get; set; } // Start time of the break (e.g., 2:00 PM)
  public TimeSpan BreakEndTime { get; set; } // End time of the break (e.g., 3:00 PM)
  public required TimeSpan ClosingTime { get; set; } // Closing time (e.g., 10:00 PM)}
  public required List<DayOfWeek> OpenDays { get; set; } = new List<DayOfWeek>(); // Days the restaurant is open
}

public class ResponseGetRestaurantsDto : ResponseRestaurantDto;

public class ResponseGetRestaurantDto : ResponseRestaurantDto;

public class ResponseCreateRestaurantDto : ResponseRestaurantDto;
public class ResponseUpdateRestaurantDto : ResponseRestaurantDto;

public class ResponseDeleteRestaurantDto
{
  public required Guid RestaurantId { get; set; }
}