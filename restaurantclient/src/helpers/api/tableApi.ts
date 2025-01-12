import {TableObject} from "../models/table.ts";
import {ReservationObject} from "../models/reservation.ts";

const BASE_URL = 'http://localhost:5101/api/Table'; // Replace with your backend URL

export async function fetchAllTables() {
  const response = await fetch(`${BASE_URL}/GetAll`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<TableObject[]>;
};

export async function fetchReservationsByTableId(tableId: string) {
  const response = await fetch(`${BASE_URL}/Reservation/GetReservationsByTableId/?TableId=${tableId}`);
  if (response.status === 404) {
    throw new Error('No reservations found for this table');
  }
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<ReservationObject[]>;
}