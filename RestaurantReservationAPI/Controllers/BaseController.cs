using AutoMapper;
using RestaurantReservationAPI.Interface;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T, TResponseGetAllDto, TResponseGetDto, TRequestCreateDto, TResponseCreateDto, TRequestUpdateDto,
  TResponseUpdateDto, TRequestDeleteDto>(IBaseRepository<T> repository, IMapper mapper)
  : ControllerBase
  where T : class
  where TResponseGetAllDto : class
  where TResponseGetDto: class
  where TRequestCreateDto : class
  where TResponseCreateDto : class
  where TRequestUpdateDto : class
  where TResponseUpdateDto : class
  where TRequestDeleteDto : class, GenericAdditions.IHasId
{
  [HttpGet("GetAll")]
  public async Task<ActionResult<TResponseGetAllDto>> GetAll()
  {
    var entities = await repository.GetAllAsync();
    var entitiesDto = mapper.Map<TResponseGetAllDto>(entities);
    return Ok(entitiesDto);
  }

  [HttpGet("GetById")]
  public async Task<ActionResult<TResponseGetDto>> GetById(Guid id)
  {
    var entity = await repository.GetByIdAsync(id);
    if (entity == null) return NotFound();

    var entityDto = mapper.Map<TResponseGetDto>(entity);
    return Ok(entityDto);
  }

  [HttpPost("Create")]
  public async Task<ActionResult<TResponseCreateDto>> Create([FromBody] TRequestCreateDto createDto)
  {
    var entity = mapper.Map<T>(createDto);
    var createdEntity = await repository.CreateAsync(entity);
    var createdEntityDto = mapper.Map<TResponseCreateDto>(createdEntity);
    return Ok(createdEntityDto);
  }

  [HttpPut("Update")]
  public async Task<ActionResult<TResponseUpdateDto>> Update([FromBody] TRequestUpdateDto updateDto)
  {
    var entity = mapper.Map<T>(updateDto);
    var updatedEntity = await repository.UpdateEntityByIdAsync(entity);
    if (updatedEntity == null) return NotFound();
    var updatedEntityDto = mapper.Map<TResponseUpdateDto>(updatedEntity);
    return Ok(updatedEntityDto);
  }

  [HttpDelete("Delete")]
  public async Task<ActionResult> Delete(TRequestDeleteDto deleteDto)
  {
    var isDeleted = await repository.DeleteByIdAsync(deleteDto.Id);
    if (!isDeleted) return NotFound();
    return Ok();
  }
}