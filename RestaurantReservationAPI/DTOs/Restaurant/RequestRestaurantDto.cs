using RestaurantReservationAPI.DTOs.Generic;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.DTOs.Restaurants;

public class RequestRestaurantDto
{
  public required string Name { get; set; }
  public required TimeSpan OpeningTime { get; set; } // Opening time (e.g., 10:00 AM)
  public TimeSpan BreakStartTime { get; set; } // Start time of the break (e.g., 2:00 PM)
  public TimeSpan BreakEndTime { get; set; } // End time of the break (e.g., 3:00 PM)
  public required TimeSpan ClosingTime { get; set; } // Closing time (e.g., 10:00 PM)}
  public required List<DayOfWeek> OpenDays { get; set; } = new List<DayOfWeek>(); // Days the restaurant is open
}

public class RequestCreateRestaurantDto : RequestRestaurantDto;

public class RequestUpdateRestaurantDto : RequestRestaurantDto
{
  public required Guid RestaurantId { get; set; }
};

public class RequestDeleteRestaurantDto : Generics.IHasId
{
  public Guid Id { get; set; }
}

public class RequestGetRestaurantDto
{
  public required Guid RestaurantId { get; set; }
}

public class RequestGetRestaurantsDto;

public class RequestUpdateRestaurantOpeningHoursDto
{
  public required Guid RestaurantId { get; set; }
  public required TimeSpan OpeningTime { get; set; } // Opening time (e.g., 10:00 AM)
  public TimeSpan BreakStartTime { get; set; } // Start time of the break (e.g., 2:00 PM)
  public TimeSpan BreakEndTime { get; set; } // End time of the break (e.g., 3:00 PM)
  public required TimeSpan ClosingTime { get; set; } // Closing time (e.g., 10:00 PM)}
}
