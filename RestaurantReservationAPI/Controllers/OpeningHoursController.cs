using AutoMapper;
using RestaurantReservationAPI.DTOs.OpeningHours;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

public class OpeningHoursController (IOpeningHoursRepository openingHoursRepository, IMapper mapper) : BaseController<OpeningHours, ResponseGetAllOpeningHoursDto, ResponseGetOpeningHoursDto, RequestCreateOpeningHoursDto, ResponseCreateOpeningHoursDto, RequestUpdateOpeningHoursDto, ResponseUpdateOpeningHoursDto, RequestDeleteOpeningHoursDto>(openingHoursRepository, mapper)
{

}