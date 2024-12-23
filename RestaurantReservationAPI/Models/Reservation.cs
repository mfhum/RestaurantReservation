using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationAPI.Models;

public class Reservation
{
  [Key] public required Guid ReservationId { get; set; } // Primary Key
  public required Guid TableId { get; set; } // Foreign Key to Table
  public required Table Table { get; set; } // Navigation Property
  public required DateTime ReservationDate { get; set; } // Date and time of the reservation
  public required int Guests { get; set; } // Number of guests
  public string? Notes { get; set; } = string.Empty;// Optional notes (e.g., special requests)
}