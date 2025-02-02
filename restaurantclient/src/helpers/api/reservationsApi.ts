import {ReservationObject} from "../models/reservation.ts";
import axios from "axios";

const BASE_URL = 'http://localhost:5101/api/Reservation'; // Replace with your backend URL

export const fetchAllReservations = async () => {
  const response = await fetch(`${BASE_URL}/GetAll`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<ReservationObject[]>;
};

export const createReservation = async (
    reservation: ReservationObject
): Promise<ReservationObject> => {
  try {
    const response = await axios.post(`${BASE_URL}/Create`, reservation);
    return response.data as ReservationObject;
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
};
