using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.DTOs.Reservations;

public class ResponseReservationDto
{
  public required Guid ReservationId { get; set; }
  public required Guid TableId { get; set; }
  public required Table Table { get; set; } // Navigation Property
  public required DateTime ReservationDate { get; set; }
  public required int Guests { get; set; }
  public string? Notes { get; set; } = string.Empty;
}

public class ResponseCreateReservationDto : ResponseReservationDto;

public class ResponseUpdateReservationDto : ResponseReservationDto;

public class ResponseCancelReservationDto
{
  public required Guid ReservationId { get; set; }
}

public class ResponseGetReservationDto : ResponseReservationDto;


public class ResponseGetReservationsDto
{
  public required List<ResponseReservationDto> Reservations { get; set; } = new List<ResponseReservationDto>();
}

public class ResponseGetReservationsByTableDto
{
  public required Guid TableId { get; set; }
  public required List<ResponseReservationDto> Reservations { get; set; } = new List<ResponseReservationDto>();
}