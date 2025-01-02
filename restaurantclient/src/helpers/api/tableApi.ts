import {TableObject} from "../models/table.ts";

const BASE_URL = 'http://localhost:5101/api'; // Replace with your backend URL

export const fetchAllTables = async () => {
  const response = await fetch(`${BASE_URL}/Table/GetAll`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return await response.json() as Promise<TableObject[]>;
};
