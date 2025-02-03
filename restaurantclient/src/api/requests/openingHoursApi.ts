import {OpeningHoursObject} from "../models/openinghours.ts";
import axios from 'axios';

const BASE_URL = 'http://localhost:5101/api/OpeningHours'; // Replace with your backend URL


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


