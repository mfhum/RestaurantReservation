using AutoMapper;
using RestaurantReservationAPI.DTOs.Availability;
using RestaurantReservationAPI.DTOs.Reservations;
using RestaurantReservationAPI.DTOs.Tables;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Reservation, RequestReservationDto>().ReverseMap();
            CreateMap<Reservation, RequestCreateReservationDto>().ReverseMap();
            CreateMap<Reservation, RequestUpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, RequestCancelReservationDto>().ReverseMap();
            CreateMap<Reservation, RequestGetReservationDto>().ReverseMap();
            CreateMap<Reservation, RequestGetReservationsByTableDto>().ReverseMap();
            CreateMap<Reservation, RequestGetReservationsByTimeRangeDto>().ReverseMap();
            CreateMap<Reservation, RequestUpdateTableSeatsDto>().ReverseMap();

            CreateMap<Reservation, ResponseReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseCreateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseUpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseCancelReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsByTableDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsByTimeRangeDto>().ReverseMap();

            CreateMap<Table, RequestTableDto>().ReverseMap();
            CreateMap<Table, RequestCreateTableDto>().ReverseMap();
            CreateMap<Table, RequestUpdateTableDto>().ReverseMap();
            CreateMap<Table, RequestDeleteTableDto>().ReverseMap();
            CreateMap<Table, RequestGetTableDto>().ReverseMap();
            CreateMap<Table, RequestGetTablesDto>().ReverseMap();

            CreateMap<Table, ResponseTableDto>().ReverseMap();
            CreateMap<Table, ResponseGetTableDto>().ReverseMap();
            CreateMap<Table, ResponseGetTablesDto>().ReverseMap();
            CreateMap<Table, ResponseCreateTableDto>().ReverseMap();
            CreateMap<Table, ResponseUpdateTableDto>().ReverseMap();
            CreateMap<Table, ResponseDeleteTableDto>().ReverseMap();

            CreateMap<Availability, ResponseGetGeneralAvailabilityDto>().ReverseMap();
        }
    }
}