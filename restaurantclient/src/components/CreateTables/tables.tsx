import classes from './tables.module.sass';
import {useMutation, useQuery} from "@tanstack/react-query";
import {createTable, fetchGetAllTables} from "../../helpers/api/tableApi.ts";
import {TableObject} from "../../helpers/models/table.ts";
import React from "react";

function Tables() {
    const GetAllTablesQuery= useQuery({
        queryKey: ['GetAllTablesQuery'],
        queryFn: fetchGetAllTables,
        enabled: true,
    })

    const CreateTableQuery = useMutation<TableObject, Error, TableObject>({
        mutationFn: createTable,
        onSuccess: () => {
            GetAllTablesQuery.refetch();
        },
    });

    const handleCreateTableFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const newTable: TableObject = {
            seats: event.currentTarget.seats.value,
            tableNumber: event.currentTarget.tableNumber.value,
        };
        CreateTableQuery.mutate(newTable);
        alert('Opening Hours Submitted');
        event.currentTarget.reset();
    }

    return (
        <>
            <section className={classes.tableSetup}>
                
                <div className={classes.tableSetup}>
                    <h2>All Tables:</h2>
                    {GetAllTablesQuery.data?.map((table: TableObject) => (
                        <div className={classes.reservationInfo} key={table.tableId}>
                            Table number: {table.tableNumber}<br/>
                            Amount of guests: {table.seats}<br/>
                        </div>
                    ))}
                    <div>{GetAllTablesQuery.isFetching ? 'Updating...' : ''}</div>
                </div>
                <div className={classes.tableCreation}>
                    <h2>Create a new table:</h2>
                    <form onSubmit={handleCreateTableFormSubmit}>
                        <label>
                            Table number:
                            <input type="number" name="tableNumber" />
                        </label>
                        <label>
                            Amount of seats:
                            <input type="number" name="seats" />
                        </label>
                        <button type="submit">Create table</button>
                    </form>
                </div>
            </section>
        </>
    )
}

export default Tables
