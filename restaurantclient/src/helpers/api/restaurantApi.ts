import {ReservationObject} from "../models/reservation.ts";
import {RestaurantObject} from "../models/restaurant.ts";

const BASE_URL = 'http://localhost:5101/api/Restaurant'; // Replace with your backend URL

export const createRestaurant = async (restaurant: RestaurantObject) => {
  const response = await fetch(`${BASE_URL}/Create`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(restaurant),
  });
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<ReservationObject[]>;
};
