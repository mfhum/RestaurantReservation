import classes from './tablesForm.module.sass';
import {useMutation, useQuery} from "@tanstack/react-query";
import {createTable, fetchGetAllTables} from "../../helpers/api/tableApi.ts";
import {TableObject} from "../../helpers/models/table.ts";
import React from "react";

function TablesForm() {
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
        event.currentTarget.reset();
    }

    return (
        <>
            <section className={classes.tableSetup}>
                <div className={classes.tableCreation}>
                    <h2>Neuen Tisch erstellen:</h2>
                    <form onSubmit={handleCreateTableFormSubmit}>
                        <label>
                            <p>Tischnummer:</p>
                            <input type="number" name="tableNumber" />
                        </label>
                        <label>
                            <p>Anzahl Sitzplätze:</p>
                            <input type="number" name="seats" />
                        </label>
                        <button type="submit"><p>Tisch erstellen</p></button>
                    </form>
                </div>
                <div className={classes.tableList}>
                    <h2>Alle Tische:</h2>
                    {GetAllTablesQuery.data?.map((table: TableObject) => (
                        <div className={classes.reservationInfo} key={table.tableId}>
                            <p>Tischnummer: {table.tableNumber}</p>
                            <p>Sitzplätze: {table.seats}</p>
                        </div>
                    ))}
                    <div>{GetAllTablesQuery.isFetching ? 'Updating...' : ''}</div>
                </div>
            </section>
        </>
    )
}

export default TablesForm
