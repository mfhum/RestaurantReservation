using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.DTOs.Tables;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

public class TableController(ITableRepository tableRepository, IMapper mapper) : BaseController<Table, ResponseGetTablesDto, ResponseGetTableDto, RequestCreateTableDto, ResponseCreateTableDto, RequestUpdateTableDto,
  ResponseUpdateTableDto, RequestDeleteTableDto>(tableRepository, mapper)
{
  [HttpPatch("UpdateTableSize")]
  public async Task<ActionResult> Create([FromBody] RequestUpdateTableSeatsDto updateSeatsDto)
  {
    var createdEntity = await tableRepository.UpdateTableSeats(updateSeatsDto.TableId, updateSeatsDto.Seats);
    return Ok("Successfully updated table size to " + updateSeatsDto.Seats);
  }
}