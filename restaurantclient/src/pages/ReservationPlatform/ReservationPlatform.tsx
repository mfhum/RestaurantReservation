import { useState, useEffect } from "react";
import CustomDatePicker from "../../components/FormObjects/CustomDatePicker.tsx";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAvailabilityForDay, getAvailabilityForMonth } from "../../helpers/api/availabilityApi.ts";
import classes from "./ReservationPlatform.module.sass";
import CustomTimePicker from "../../components/FormObjects/CustomTimePicker.tsx";
import {formatDate} from "../../helpers/functions/dateTime.ts";

function ReservationPlatform() {
  const [guestCount, setGuestCount] = useState(2);
  const [reservationTime, setReservationTime] = useState(new Date().toISOString());
  const [selectedDate, setSelectedDate] = useState<string>('');
  const [selectedTime, setSelectedTime] = useState<string>('');
  const queryClient = useQueryClient();

  const GetAvailabilityForMonthQuery = useQuery({
    queryKey: ['GetAllOpeningHours', reservationTime, guestCount],
    queryFn: () => getAvailabilityForMonth(reservationTime, guestCount),
    enabled: true,
  });

  const GetAvailabilityByDayQuery = useQuery({
    queryKey: ['GetAvailabilityByDay', selectedDate, guestCount],
    queryFn: () => getAvailabilityForDay(selectedDate, guestCount),
    enabled: !!selectedDate, // Enable query only if selectedDate is not empty
  });

  useEffect(() => {
    if (selectedDate) {
      queryClient.invalidateQueries({ queryKey: ['GetAvailabilityByDay', selectedDate, guestCount] });    }
  }, [selectedDate, guestCount, queryClient]);

  function handleDateSelect(newSelectedDate: string) {
    const date = new Date(newSelectedDate);
    date.setHours(date.getHours() + 1); // Add one hour
    setSelectedDate(date.toISOString());
    console.log(date.toISOString());
  }

  return (
      <section className={classes.reservationPlatform}>
        <div>
          <label htmlFor="person-count">Für wieviele Personen ist die Reservierung?</label>
          <div>
            <button
                id="decrease"
                className="btn"
                onClick={() => setGuestCount(guestCount - 1)}
                disabled={guestCount == 2}
            >
              −
            </button>
            <span id="count" className="count">{guestCount}</span>
            <button
                id="increase"
                className="btn"
                onClick={() => setGuestCount(guestCount + 1)}
                disabled={guestCount == 10}
            >
              +
            </button>
          </div>
        </div>
        {GetAvailabilityForMonthQuery.data && (
            <CustomDatePicker
                availability={GetAvailabilityForMonthQuery.data}
                reservationTime={reservationTime}
                onMonthChange={setReservationTime} // Pass the setter function
                onDayChange={(newSelectedDate) => handleDateSelect(newSelectedDate)} // Pass the setter function
            />
        )}
        {selectedDate ? new Date(selectedDate).toLocaleDateString('de-DE') : 'No date selected'}
        <CustomTimePicker availableTimes={Array.isArray(GetAvailabilityByDayQuery.data) ? GetAvailabilityByDayQuery.data : []} onSelectTime={setSelectedTime} />
      </section>
  );
}

export default ReservationPlatform;