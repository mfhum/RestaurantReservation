using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationAPI.Models;

public class Table
{
  [Key] public required Guid TableId { get; set; } // Primary Key
  public required int TableNumber { get; set; } // Unique table identifier
  public required int Seats { get; set; } // Number of seats available

  // Navigation property for the one-to-many relationship
  public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}