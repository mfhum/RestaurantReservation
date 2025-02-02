export interface ReservationObject {
  guests: number;
  notes?: string;
  reservationDate: string; // ISO 8601 format (e.g., "2025-01-01T23:00:00.694Z")
  reservationId?: string;   // UUID
  tableId?: string;         // UUID
}
