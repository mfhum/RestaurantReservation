using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.DTOs.Reservations;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

public class ReservationController(IReservationRepository reservationRepository, IMapper mapper) : BaseController<Reservation, ResponseGetReservationsDto, ResponseGetReservationDto, RequestCreateReservationDto, ResponseCreateReservationDto, RequestUpdateReservationDto,
  ResponseUpdateReservationDto, RequestCancelReservationDto>(reservationRepository, mapper)
{
  private readonly IMapper _mapper = mapper;

  [HttpGet("GetReservationsByTimeRange")]
  public async Task<ActionResult<ICollection<Reservation>>> GetReservationsByTimeRange([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
  {
    var reservations = await reservationRepository.GetReservationsByTimeRange(startTime, endTime);
    var reservationsDto = _mapper.Map<List<ResponseGetReservationsByTimeRangeDto>>(reservations);
    return Ok(reservationsDto);
  }

  [HttpGet("GetReservationsByTableId")]
  public async Task<ActionResult<ICollection<Reservation>>> GetReservationsByTableId([FromQuery] RequestGetReservationsByTableDto requestGetReservationsByTableDto)
  {
    var reservations = await reservationRepository.GetReservationsByTableId(requestGetReservationsByTableDto.TableId);
    var reservationsDto = _mapper.Map<List<ResponseGetReservationsByTableDto>>(reservations);
    return Ok(reservationsDto);
  }
  
  [HttpPost("CreateReservationByGuestNumber")]
  public async Task<ActionResult<ResponseCreateReservationByGuestNumberDto>> CreateReservationByGuestNumber([FromBody] RequestCreateReservationByGuestNumberDto requestCreateReservationByGuestNumberDto)
  {
    var reservation = _mapper.Map<Reservation>(requestCreateReservationByGuestNumberDto);
    var createdReservation = await reservationRepository.CreateReservationByGuestNumber(reservation);
    var createdReservationDto = _mapper.Map<ResponseCreateReservationByGuestNumberDto>(createdReservation);
    return Ok(createdReservationDto);
  }
  

}