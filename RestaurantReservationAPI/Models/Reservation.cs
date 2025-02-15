using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace RestaurantReservationAPI.Models;

public class Reservation
{
  [Key] public required Guid ReservationId { get; set; } // Primary Key
  public required Guid TableId { get; set; } // Foreign Key to Table
  public required Table Table { get; set; } // Navigation Property
  public required DateTime ReservationDate { get; set; } // Date and time of the reservation
  public required int Guests { get; set; } // Number of guests
  public string? Notes { get; set; } = string.Empty;// Optional notes (e.g., special requests)
  public string? Mail { get; set; } = string.Empty; // Optional email address
  public string Name { get; set; } = string.Empty; // Optional name of the person who made the reservation

}