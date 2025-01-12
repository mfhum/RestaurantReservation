import { useState } from "react";
import CustomDatePicker from "../../components/FormObjects/CustomDatePicker.tsx";
import { useQuery } from "@tanstack/react-query";
import { getAvailabilityForMonth } from "../../helpers/api/availabilityApi.ts";
import classes from "./ReservationPlatform.module.sass";

function ReservationPlatform() {
  const [guestCount, setGuestCount] = useState(2);
  const [reservationTime, setReservationTime] = useState(new Date().toISOString());

  const GetAvailabilityForMonthQuery = useQuery({
    queryKey: ['GetAllOpeningHours', reservationTime, guestCount],
    queryFn: () => getAvailabilityForMonth(reservationTime, guestCount),
    enabled: true,
  });

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
            />
        )}
      </section>
  );
}

export default ReservationPlatform;
