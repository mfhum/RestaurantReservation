import { useState, useEffect } from "react";
import CustomDatePicker from "../../../components/FormObjects/DatePicker/CustomDatePicker.tsx";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getAvailabilityForDay, getAvailabilityForMonth } from "../../../api/requests/availabilityApi.ts";
import classes from "./ReservationPlatform.module.sass";
import CustomTimePicker from "../../../components/FormObjects/TimePicker/CustomTimePicker.tsx";
import { ReservationObject } from "../../../api/models/reservation.ts";
import { createReservation } from "../../../api/requests/reservationsApi.ts";

function ReservationPlatform() {
  const [guestCount, setGuestCount] = useState(2);
  const [reservationTime, setReservationTime] = useState(new Date().toISOString());
  const [selectedDate, setSelectedDate] = useState<string>('');
  const [selectedTime, setSelectedTime] = useState<string>('');
  const [isMonthChanging, setIsMonthChanging] = useState(false); // Add state to track month change
  const queryClient = useQueryClient();

  const GetAvailabilityForMonthQuery = useQuery({
    queryKey: ['GetAllOpeningHours', reservationTime, guestCount],
    queryFn: () => getAvailabilityForMonth(reservationTime, guestCount),
    enabled: true
  });

  const GetAvailabilityByDayQuery = useQuery({
    queryKey: ['GetAvailabilityByDay', selectedDate, guestCount],
    queryFn: () => getAvailabilityForDay(selectedDate, guestCount),
    enabled: !!selectedDate, // Enable query only if selectedDate is not empty
  });

  const CreateReservation = useMutation<ReservationObject, Error, ReservationObject>({
    mutationFn: createReservation,
    onSuccess: () => {
      GetAvailabilityForMonthQuery.refetch();
    },
  });

  useEffect(() => {
    if (selectedDate) {
      queryClient.invalidateQueries({ queryKey: ['GetAvailabilityByDay', selectedDate, guestCount] });
    }
  }, [selectedDate, guestCount, queryClient]);

  useEffect(() => {
    // Reset data when guestCount changes
    setReservationTime(new Date().toISOString());
    setSelectedDate('');
    setSelectedTime('');
  }, [guestCount]);

  function handleDateSelect(newSelectedDate: string) {
    const date = new Date(newSelectedDate);
    date.setHours(date.getHours() + 1); // Add one hour
    setSelectedDate(date.toISOString());
    setIsMonthChanging(true);
    setIsMonthChanging(false);
  }

  function handleTimeSelect(newSelectedTime: string) {
    const date = new Date(newSelectedTime);
    setSelectedTime(date.toISOString());
  }

  function handleReservation() {
    const newReservation: ReservationObject = {
      guests: guestCount,
      reservationDate: selectedTime,
    }

    CreateReservation.mutate(newReservation);
    alert('Reservation Submitted');
    setGuestCount(2);
    setReservationTime(new Date().toISOString());
    setSelectedDate('');
    setSelectedTime('');
  };

  function handleMonthChange(newMonth: string) {
    setIsMonthChanging(true); // Set month changing state to true
    setReservationTime(newMonth);
  }

  return (
      <section className={classes.reservationPlatform}>
        <h1 className={classes.title}>Restaurant Reservation Tool</h1>
        <div>
          <label htmlFor="person-count"><p>Für wieviele Personen ist die Reservierung?</p></label>
          <div className={classes.guestCountSelector}>
            <button
                className={classes.buttonCreasers}
                id="decrease"
                onClick={() => setGuestCount(guestCount - 1)}
                disabled={guestCount == 1}
            >
              <p>−</p>
            </button>
            <span id="count" className="count"><h3>{guestCount} {guestCount === 1 ? "Gast" : "Gäste"}</h3></span>
            <button
                className={classes.buttonCreasers}
                id="increase"
                onClick={() => setGuestCount(guestCount + 1)}
                disabled={guestCount == 10}
            >
              <p>+</p>
            </button>
          </div>
        </div>
        {GetAvailabilityForMonthQuery.data && (
            <CustomDatePicker
                availability={GetAvailabilityForMonthQuery.data}
                reservationTime={reservationTime}
                onMonthChange={handleMonthChange} // Use the new handler
                onDayChange={(newSelectedDate) => handleDateSelect(newSelectedDate)} // Pass the setter function
            />
        )}
        {!isMonthChanging && (
            <CustomTimePicker
                availableTimes={Array.isArray(GetAvailabilityByDayQuery.data) ? GetAvailabilityByDayQuery.data : []}
                onSelectTime={(newSelectedTime) => handleTimeSelect(newSelectedTime)}
            />
        )}
        { selectedDate && selectedTime && (
            <>
              <h2>Deine Reservation am {new Date(selectedTime).toLocaleDateString('de-DE', { day: 'numeric', month: 'long' })} um {new Date(selectedTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false })} für {guestCount} {guestCount === 1 ? "Gast" : "Gäste"}</h2>
              <button className={classes.button} onClick={() => handleReservation()}><p>Reservation abschicken</p></button>
            </>
        )}
      </section>
  );
}

export default ReservationPlatform;