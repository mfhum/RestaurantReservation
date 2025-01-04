namespace RestaurantReservationAPI.DTOs.Tables;

public class ResponseRestaurantDto
{
  public required Guid RestaurantId { get; set; }
  public required string Name { get; set; }
}

public class ResponseGetRestaurantsDto : ResponseRestaurantDto;

public class ResponseGetRestaurantDto : ResponseRestaurantDto;

public class ResponseCreateRestaurantDto : ResponseRestaurantDto;
public class ResponseUpdateRestaurantDto : ResponseRestaurantDto;

public class ResponseDeleteRestaurantDto
{
  public required Guid RestaurantId { get; set; }
}