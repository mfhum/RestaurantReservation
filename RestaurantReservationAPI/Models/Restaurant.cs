using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationAPI.Models;

public class Restaurant
{
  [Key] [Required] public required Guid RestaurantId { get; set; }
  [Required] public string Name { get; set; }
}