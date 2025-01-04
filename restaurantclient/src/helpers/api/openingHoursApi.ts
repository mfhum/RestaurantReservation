import {OpeningHoursObject} from "../models/openinghours.ts";

const BASE_URL = 'http://localhost:5101/api/OpeningHours'; // Replace with your backend URL

export const getAllOpeningHours = async () => {
  const response = await fetch(`${BASE_URL}/GetAll`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<OpeningHoursObject[]>;
};
