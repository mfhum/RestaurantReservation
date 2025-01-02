import { useQuery } from '@tanstack/react-query';
import classes from './AllTables.module.sass';
import {fetchAllTables} from "../../helpers/api/tableApi.ts";
import {TableObject} from "../../helpers/models/table.ts";

function AllTables() {
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
            <div className={classes.tableInfo} key={table.seats + table.tableNumber + table.tableId}>
              Amount of seats: {table.seats}<br/>
              Table Number: {table.tableNumber}<br/>
            </div>
        ))}
        <div>{AllTablesQuery.isFetching ? 'Updating...' : ''}</div>
      </div>
  )
}

export default AllTables;