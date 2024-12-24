namespace RestaurantReservationAPI.Models;

public class GenericAdditions
{
  public interface IHasId
  {
    Guid Id { get; set; } // Or use int, depending on your use case
  }
}