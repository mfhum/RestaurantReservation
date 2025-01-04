using RestaurantReservationAPI.DTOs.Generic;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.DTOs.Restaurants;

public class RequestRestaurantDto
{
  public string Name { get; set; }
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