using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.DTOs.Restaurants;
using RestaurantReservationAPI.DTOs.Tables;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers;

public class RestaurantController(IRestaurantRepository restaurantRepository, IMapper mapper) : BaseController<Restaurant, ResponseGetRestaurantsDto, ResponseGetRestaurantDto, RequestCreateRestaurantDto, ResponseCreateRestaurantDto, RequestUpdateRestaurantDto, ResponseUpdateRestaurantDto, RequestDeleteRestaurantDto>(restaurantRepository, mapper);