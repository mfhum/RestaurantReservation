using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.DTOs.OpeningHours;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

public class OpeningHoursController (IOpeningHoursRepository openingHoursRepository, IMapper mapper) : BaseController<OpeningHours, ResponseGetAllOpeningHoursDto, ResponseGetOpeningHoursDto, RequestCreateOpeningHoursDto, ResponseCreateOpeningHoursDto, RequestUpdateOpeningHoursDto, ResponseUpdateOpeningHoursDto, RequestDeleteOpeningHoursDto>(openingHoursRepository, mapper)
{
  private readonly IMapper _mapper = mapper;

  [HttpDelete("DeleteByDay")]
  public async Task<ActionResult<ICollection<Reservation>>> GetReservationsByTimeRange(DayOfWeek weekday)
  {
    var isDeleted = await openingHoursRepository.DeleteByWeekday(weekday);
    if (!isDeleted) return NotFound();
    return Ok();
  }
}