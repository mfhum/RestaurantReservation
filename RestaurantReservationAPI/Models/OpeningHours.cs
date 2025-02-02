using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationAPI.Models;

public class OpeningHours
{
  [Key] public Guid OpeningHoursId { get; set; }
  [Required] public DayOfWeek Day { get; set; }
  [Required] public TimeSpan OpeningTime { get; set; }
  public TimeSpan? BreakStartTime { get; set; }
  public TimeSpan? BreakEndTime { get; set; }
  [Required] public TimeSpan ClosingTime { get; set; }
}