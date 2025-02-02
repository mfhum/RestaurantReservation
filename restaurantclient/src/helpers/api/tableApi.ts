import {TableObject} from "../models/table.ts";
import {ReservationObject} from "../models/reservation.ts";
import axios from "axios";

const BASE_URL = 'http://localhost:5101/api/Table'; // Replace with your backend URL

export async function fetchGetAllTables() {
  const response = await fetch(`${BASE_URL}/GetAll`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<TableObject[]>;
}

export const createTable = async (
    table: TableObject
): Promise<TableObject> => {
  try {
    const response = await axios.post(`${BASE_URL}/Create`, table);
    return response.data as TableObject;
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(error.message);
    }
    throw new Error('An unknown error occurred');
  }
}

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