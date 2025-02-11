import {ReservationObject} from "../models/reservation.ts";
import axios from "axios";

const API_BASE_URL = import.meta.env.VITE_API_URL || "https://restaurant.marius.li:3020";
const BASE_URL = API_BASE_URL + '/Reservation'; // Replace with your backend URL

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
    const response = await axios.post(`${BASE_URL}/CreateReservationByGuestNumber`, reservation);
    return response.data as ReservationObject;
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
};
