namespace RestaurantReservationAPI.DTOs.Generic;

public class Generics
{
  public interface IHasId
  {
    Guid Id { get; set; } // Or use int, depending on your use case
  }
}