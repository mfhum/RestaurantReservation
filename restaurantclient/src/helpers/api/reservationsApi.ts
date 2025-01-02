import {ReservationObject} from "../models/reservation.ts";

const BASE_URL = 'http://localhost:5101/api'; // Replace with your backend URL

export const fetchAllReservations = async () => {
  const response = await fetch(`${BASE_URL}/Reservation/GetAll`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<ReservationObject[]>;
};
