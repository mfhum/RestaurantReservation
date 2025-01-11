using AutoMapper;
using RestaurantReservationAPI.Interface;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.DTOs.Availability;

namespace RestaurantReservationAPI.Controllers
{
  [ApiController]
  [Route("api/Availability")]
  public class AvailabilityController(IAvailabilityRepository availabilityRepository, IMapper mapper)
    : ControllerBase
  {
    [HttpGet("GetGeneralAvailability")]
    public async Task<ActionResult<List<ResponseGetGeneralAvailabilityDto>>> GetGeneralAvailability([FromQuery] RequestGetGeneralAvailabilityDto generalAvailabilityDto)
    {
      var reservations = await availabilityRepository.GetAvailabilityByTimeAndGuests(
        generalAvailabilityDto.ReservationTime,
        generalAvailabilityDto.NumberOfGuests
      );
      var reservationsDto = mapper.Map<List<ResponseGetGeneralAvailabilityDto>>(reservations);
      return Ok(reservationsDto);
    }

    [HttpGet("GetAvailabilityForMonth")]
    public async Task<ActionResult<List<ResponseGetAvailabilityForMonthDto>>> GetAvailabilityForMonth([FromQuery] RequestGetAvailabilityForMonthDto availabilityForMonthDto)
    {
      var reservations = await availabilityRepository.GetAvailabilityByMonth(
        availabilityForMonthDto.ReservationTime,
        availabilityForMonthDto.NumberOfGuests
      );
      var reservationsDto = mapper.Map<List<ResponseGetAvailabilityForMonthDto>>(reservations);
      return Ok(reservationsDto);
    }

  }
}