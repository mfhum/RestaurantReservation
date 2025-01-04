using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationAPI.Models;

public class Restaurant
{
  [Key] [Required] public required Guid RestaurantId { get; set; }
  [Required] public string Name { get; set; }
  [Required] public TimeSpan OpeningTime { get; set; } // Opening time (e.g., 10:00 AM)
  public TimeSpan? BreakStartTime { get; set; } // Start time of the break (e.g., 2:00 PM)
  public TimeSpan? BreakEndTime { get; set; } // End time of the break (e.g., 3:00 PM)
  [Required] public TimeSpan ClosingTime { get; set; } // Closing time (e.g., 10:00 PM)}
  [Required] public List<DayOfWeek> OpenDays { get; set; } = new List<DayOfWeek>(); // Days the restaurant is open

}