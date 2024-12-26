namespace RestaurantReservationAPI.DTOs.Tables;

public class ResponseTableDto
{
  public required Guid RestaurantId { get; set; }
  public required int TableNumber { get; set; }
  public required int Seats { get; set; }
}

public class ResponseGetTablesDto
{
  public required List<ResponseTableDto> Tables { get; set; } = new List<ResponseTableDto>();
}

public class ResponseGetTableDto : ResponseTableDto
{
  public required Guid TableId { get; set; }
}

public class ResponseCreateTableDto : ResponseTableDto
{
  public required Guid TableId { get; set; }
}

public class ResponseUpdateTableDto : ResponseTableDto
{
  public required Guid TableId { get; set; }
}

public class ResponseDeleteTableDto
{
  public required Guid TableId { get; set; }
}