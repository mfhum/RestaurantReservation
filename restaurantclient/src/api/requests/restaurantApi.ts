import {RestaurantObject} from "../models/restaurant.ts";
import axios from "axios";

const API_BASE_URL = import.meta.env.VITE_API_URL || "https://restaurant.marius.li:3020";
const BASE_URL = API_BASE_URL + '/Restaurant'; // Replace with your backend URL

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