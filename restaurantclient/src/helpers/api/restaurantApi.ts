import {RestaurantObject} from "../models/restaurant.ts";
import axios from "axios";

const BASE_URL = 'http://localhost:5101/api/Restaurant'; // Replace with your backend URL

export const createRestaurant = async (
    restaurant: RestaurantObject
): Promise<RestaurantObject> => {
  try {
    const response = await axios.post(`${BASE_URL}/Create`, restaurant);
    return response.data as RestaurantObject;
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
}