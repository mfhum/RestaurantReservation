namespace RestaurantReservationAPI.DTOs.Tables;

public class ResponseTableDto
{
  public required Guid TableId { get; set; }
  public required int TableNumber { get; set; }
  public required int Seats { get; set; }
}

public class ResponseGetTablesDto
{
  public required List<ResponseTableDto> Tables { get; set; } = new List<ResponseTableDto>();
}

public class ResponseGetTableDto : ResponseTableDto;

public class ResponseCreateTableDto : ResponseTableDto;
public class ResponseUpdateTableDto : ResponseTableDto;
public class ResponseDeleteTableDto
{
  public required Guid TableId { get; set; }
}