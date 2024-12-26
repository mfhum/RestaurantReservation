using AutoMapper;
using RestaurantReservationAPI.DTOs.Reservations;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

public class ReservationController(IReservationRepository reservationRepository, IMapper mapper) : BaseController<Reservation, ResponseGetReservationsDto, ResponseGetReservationDto, RequestCreateReservationDto, ResponseCreateReservationDto, RequestUpdateReservationDto,
  ResponseUpdateReservationDto, RequestCancelReservationDto>(reservationRepository, mapper)
{

}