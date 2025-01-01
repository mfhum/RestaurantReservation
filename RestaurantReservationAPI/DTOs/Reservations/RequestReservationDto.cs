using RestaurantReservationAPI.DTOs.Generic;

namespace RestaurantReservationAPI.DTOs.Reservations;

public class RequestReservationDto
{
    public required Guid TableId { get; set; }
    public required DateTime ReservationDate { get; set; }
    public required int Guests { get; set; }
    public string? Notes { get; set; } = string.Empty;
  }

public class RequestCreateReservationDto : RequestReservationDto;

public class RequestUpdateReservationDto : RequestReservationDto
{
  public required Guid ReservationId { get; set; }
}

public class RequestCancelReservationDto : Generics.IHasId
{
  public Guid Id { get; set; }
}
public class RequestGetReservationDto
{
  public required Guid ReservationId { get; set; }
}

public class RequestGetReservationsByTableDto
{
  public required Guid TableId { get; set; }
  public required DateTime StartDate { get; set; }
  public required DateTime EndDate { get; set; }
}

public class RequestGetReservationsByTimeRangeDto
{
  public required DateTime StartTime { get; set; }
  public required DateTime EndTime { get; set; }
}

public class RequestUpdateTableSeatsDto
{
  public required Guid TableId { get; set; }
  public required int Seats { get; set; }
}