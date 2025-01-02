import { useQuery } from '@tanstack/react-query';
import { fetchAllReservations } from "../../helpers/api/reservationsApi.ts";
import { ReservationObject } from "../../helpers/models/reservation.ts";
import { formatDate, formatTime } from "../../helpers/functions/dateTime.ts";
import classes from './Reservations.module.sass';

function Reservations() {
  const { isPending, error, data, isFetching} = useQuery({
    queryKey: ['Reservations'],
    queryFn: fetchAllReservations,
    enabled: true,
  })
  if (isPending) return 'Loading...';

  if (error) return `An error has occurred: ${error.message}`;

  console.log(data);
  return (
      <div>
        <h2>All Reservations:</h2>
        {data.map((reservation: ReservationObject) => (
            <div className={classes.reservationInfo} key={reservation.reservationId}>
              Amount of guests: {reservation.guests}<br/>
              Date of reservation: {formatDate(reservation.reservationDate)}<br/>
              Time of reservation: {formatTime(reservation.reservationDate)}<br/>
              Notes: {reservation.notes == 'string' ? 'no notes' : reservation.notes}<br/>
            </div>
        ))}
        <div>{isFetching ? 'Updating...' : ''}</div>
      </div>
  )
}

export default Reservations;