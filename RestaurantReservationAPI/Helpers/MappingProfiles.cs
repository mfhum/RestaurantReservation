using AutoMapper;
using RestaurantReservationAPI.DTOs.Availability;
using RestaurantReservationAPI.DTOs.OpeningHours;
using RestaurantReservationAPI.DTOs.Reservations;
using RestaurantReservationAPI.DTOs.Restaurants;
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
            CreateMap<Reservation, RequestGetReservationsByIdDto>().ReverseMap();
            CreateMap<Reservation, RequestCreateReservationByGuestNumberDto>().ReverseMap();
            CreateMap<Reservation, ResponseCreateReservationByGuestNumberDto>().ReverseMap();

            CreateMap<Reservation, ResponseReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseCreateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseUpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseCancelReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsByTableDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsByTimeRangeDto>().ReverseMap();
            CreateMap<Reservation, ResponseGetReservationsByIdDto>().ReverseMap();

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

            CreateMap<Restaurant, RequestRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, RequestCreateRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, RequestUpdateRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, RequestDeleteRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, RequestGetRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, RequestGetRestaurantsDto>().ReverseMap();

            CreateMap<Restaurant, ResponseRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, ResponseGetRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, ResponseGetRestaurantsDto>().ReverseMap();
            CreateMap<Restaurant, ResponseCreateRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, ResponseUpdateRestaurantDto>().ReverseMap();
            CreateMap<Restaurant, ResponseDeleteRestaurantDto>().ReverseMap();

            CreateMap<OpeningHours, RequestOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, RequestCreateOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, RequestUpdateOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, RequestDeleteOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, RequestGetOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, RequestGetAllOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, RequestDeleteOpeningHoursByWeekdayDto>().ReverseMap();

            CreateMap<OpeningHours, ResponseOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, ResponseGetOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, ResponseGetAllOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, ResponseCreateOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, ResponseUpdateOpeningHoursDto>().ReverseMap();
            CreateMap<OpeningHours, ResponseDeleteOpeningHoursDto>().ReverseMap();

            CreateMap<Availability, RequestAvailablityDto>().ReverseMap();
            CreateMap<Availability, RequestGetGeneralAvailabilityDto>().ReverseMap();
            CreateMap<Availability, RequestGetAvailabilityForMonthDto>().ReverseMap();

            CreateMap<Availability, ResponseAvailabilityDto>().ReverseMap();
            CreateMap<Availability, ResponseGetGeneralAvailabilityDto>().ReverseMap();
            CreateMap<AvailabilityForDay, ResponseGetAvailabilityForMonthDto>().ReverseMap();
        }
    }
}