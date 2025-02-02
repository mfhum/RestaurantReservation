import { useQuery } from '@tanstack/react-query';
import { fetchAllReservations } from "../../helpers/api/reservationsApi.ts";
import { ReservationObject } from "../../helpers/models/reservation.ts";
import { formatDate, formatTime } from "../../helpers/functions/dateTime.ts";
import classes from './AllReservations.module.sass';

function AllReservations() {
  const AllReservationsQuery = useQuery({
    queryKey: ['AllReservations'],
    queryFn: fetchAllReservations,
    enabled: true,
  })
  if (AllReservationsQuery.isPending) return 'Loading...';

  if (AllReservationsQuery.error) return `An error has occurred: ${AllReservationsQuery.error.message}`;

  return (
    <div>
      <h2>All Reservations:</h2>
      {AllReservationsQuery.data.map((reservation: ReservationObject) => (
        <div className={classes.reservationInfo} key={reservation.reservationId}>
          Amount of guests: {reservation.guests}<br/>
          Date of reservation: {formatDate(new Date(reservation.reservationDate))}<br/>
          Time of reservation: {formatTime(new Date(reservation.reservationDate))}<br/>
          Notes: {reservation.notes == 'string' ? 'no notes' : reservation.notes}<br/>
        </div>
      ))}
      <div>{AllReservationsQuery.isFetching ? 'Updating...' : ''}</div>
    </div>
  )
}

export default AllReservations;