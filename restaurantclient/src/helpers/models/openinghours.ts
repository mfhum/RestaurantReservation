export interface OpeningHoursObject {
  openingHoursId?: string;
  day: number;
  openingTime: string;
  breakStartTime?: string;
  breakEndTime?: string;
  closingTime: string;
  restaurantId: string;
}