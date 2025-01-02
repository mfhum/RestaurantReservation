import { useQuery } from '@tanstack/react-query';
import classes from './AllTables.module.sass';
import {fetchAllTables} from "../../helpers/api/tableApi.ts";
import {TableObject} from "../../helpers/models/table.ts";
import {useState} from "react";
import ReservationsForTable from "../ReservationPerTable/ReservationsForTable.tsx";

function AllTables() {
  const [selectedTableId, setSelectedTableId] = useState<string | null>(null);

  const AllTablesQuery= useQuery({
    queryKey: ['AllTables'],
    queryFn: fetchAllTables,
    enabled: true,
  })
  if (AllTablesQuery.isPending) return 'Loading...';

  if (AllTablesQuery.error) return `An error has occurred: ${AllTablesQuery.error.message}`;

  return (
      <div>
        <h2>All Tables:</h2>
        {AllTablesQuery.data.map((table: TableObject) => (
            <div className={classes.tableInfo} key={table.tableId}>
              <button onClick={() => setSelectedTableId(table.tableId)}>
                Amount of seats: {table.seats}<br/>
                Table Number: {table.tableNumber}<br/>
              </button>
            </div>
        ))}
        <div>
          {selectedTableId && (
              <>
                <ReservationsForTable tableId={selectedTableId}/>
                <button onClick={() => setSelectedTableId(null)}>Close</button>
              </>
          )}
        </div>
        <div>{AllTablesQuery.isFetching ? 'Updating...' : ''}</div>
      </div>
  )
}

export default AllTables;