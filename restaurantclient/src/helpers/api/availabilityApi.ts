import axios from 'axios';
import {AvailabilityByDay, AvailabilityByMonth} from "../models/availability.ts";

const BASE_URL = 'http://localhost:5101/api/Availability'; // Replace with your backend URL

export const getAvailabilityForDay = async (reservationTime: string, numberOfGuests: number): Promise<AvailabilityByDay> => {
  try {
    const response = await axios.get(`${BASE_URL}/GetGeneralAvailability`, {
      params: {
        ReservationTime: reservationTime,
        NumberOfGuests: numberOfGuests
      }
    });
    return response.data as AvailabilityByDay;
  } catch (error) {
    // Handle errors gracefully by throwing them
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
}

export const getAvailabilityForMonth = async (reservationTime: string, numberOfGuests: number): Promise<AvailabilityByMonth[]> => {
  try {
    const response = await axios.get(`${BASE_URL}/GetAvailabilityForMonth`, {
      params: {
        ReservationTime: reservationTime,
        NumberOfGuests: numberOfGuests
      }
    });
    return response.data as AvailabilityByMonth[];
  } catch (error) {
    // Handle errors gracefully by throwing them
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
};