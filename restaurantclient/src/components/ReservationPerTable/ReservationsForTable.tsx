import { useQuery } from '@tanstack/react-query';
import classes from './ReservationsForTable.module.sass';
import {fetchReservationsByTableId} from "../../helpers/api/tableApi.ts";
import {ReservationObject} from "../../helpers/models/reservation.ts";
import {formatDate, formatTime} from "../../helpers/functions/dateTime.ts";

function ReservationsForTable(props: {tableId: string}) {
  const tableId = props.tableId;

  const ReservationsByTableId= useQuery({
    queryKey: ['ReservationsByTableId', tableId],
    queryFn: () => fetchReservationsByTableId(tableId),
    enabled: !!tableId
  })
  if (ReservationsByTableId.isPending) return 'Loading...';

  if (ReservationsByTableId.error) return `An error has occurred: ${ReservationsByTableId.error.message}`;

  return (
      <div>
        {ReservationsByTableId.data.length === 0 ? 'No reservations found for this table' :
          <>
            <h2>All Reservations:</h2>
            {ReservationsByTableId.data.map((reservation: ReservationObject) => (
              <div className={classes.reservationInfo} key={reservation.reservationId}>
                Amount of guests: {reservation.guests}<br/>
                Date of reservation: {formatDate(reservation.reservationDate)}<br/>
                Time of reservation: {formatTime(reservation.reservationDate)}<br/>
                Notes: {reservation.notes == 'string' ? 'no notes' : reservation.notes}<br/>
              </div>
            ))}
          </>
        }
        <div>{ReservationsByTableId.isFetching ? 'Updating...' : ''}</div>
      </div>
  )
}

export default ReservationsForTable;