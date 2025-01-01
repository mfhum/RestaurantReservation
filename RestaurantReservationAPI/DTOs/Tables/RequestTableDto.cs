using RestaurantReservationAPI.DTOs.Generic;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.DTOs.Tables;

public class RequestTableDto
{
  public required int TableNumber { get; set; }
  public required int Seats { get; set; }
}

public class RequestCreateTableDto : RequestTableDto;

public class RequestUpdateTableDto : RequestTableDto
{
  public required Guid TableId { get; set; }
}

public class RequestDeleteTableDto : Generics.IHasId
{
  public Guid Id { get; set; }
}

public class RequestGetTableDto
{
  public required Guid TableId { get; set; }
}

public class RequestGetTablesDto;



