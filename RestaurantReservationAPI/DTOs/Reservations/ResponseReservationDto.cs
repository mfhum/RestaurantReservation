using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.DTOs.Reservations;

public class ResponseReservationDto
{
  public required Guid ReservationId { get; set; }
  public required Guid TableId { get; set; }
  public required DateTime ReservationDate { get; set; }
  public required int Guests { get; set; }
  public string? Notes { get; set; } = string.Empty;
  public string? Mail { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
}

public class ResponseCreateReservationDto : ResponseReservationDto;

public class ResponseUpdateReservationDto : ResponseReservationDto;

public class ResponseCancelReservationDto
{
  public required Guid ReservationId { get; set; }
}

public class ResponseGetReservationDto : ResponseReservationDto;


public class ResponseGetReservationsDto : ResponseReservationDto;

public class ResponseGetReservationsByTableDto : ResponseReservationDto;
public class ResponseGetReservationsByTimeRangeDto : ResponseReservationDto;

public class ResponseGetReservationsByIdDto : ResponseReservationDto;

public class ResponseCreateReservationByGuestNumberDto : ResponseReservationDto;