import {OpeningHoursObject} from "../models/openinghours.ts";
import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || "https://restaurant.marius.li:3020";
const BASE_URL = API_BASE_URL + '/OpeningHours'; // Replace with your backend URL


export const getAllOpeningHours = async (): Promise<OpeningHoursObject[]> => {
  try {
    const response = await axios.get(`${BASE_URL}/GetAll`);
    return response.data as OpeningHoursObject[];
  } catch (error) {
    // Handle errors gracefully by throwing them
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
};


export const createOpeningHours = async (
    openingHours: OpeningHoursObject
): Promise<OpeningHoursObject> => {
  try {
    const response = await axios.post(`${BASE_URL}/Create`, openingHours);
    return response.data as OpeningHoursObject;
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
};


