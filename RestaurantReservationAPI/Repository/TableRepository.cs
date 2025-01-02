// Update TableRepository.cs
using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class TableRepository(DataContext context, IReservationRepository reservationRepository)
    : BaseRepository<Table>(context), ITableRepository
{
    private readonly DataContext _context = context;

    public new async Task<Table?> CreateAsync(Table newTable)
    {
        // check if table already exists by table number
        var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == newTable.TableNumber);
        if (table != null)
        {
            throw new ArgumentException("Table already exists");
        }
        // check if enough seats
        if (newTable.Seats <= 0)
        {
            throw new ArgumentException("Table must have at least 1 seat");
        }
        await _context.Tables.AddAsync(newTable);
        await _context.SaveChangesAsync();
        return newTable;
    }
    public async Task<bool> UpdateTableSeats(Guid tableId, int newSeatNumber)
    {
        var table = await _context.Tables.FindAsync(tableId);
        if (table == null) return false;
        // check if enough seats
        if (newSeatNumber <= 0)
        {
            throw new ArgumentException("Table must have at least 1 seat");
        }
        // check if table number already exists on other table
        var tableWithSameNumber = await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == table.TableNumber);
        if (tableWithSameNumber != null && tableWithSameNumber.TableId != tableId)
        {
            throw new ArgumentException("Table number already exists");
        }
        table.Seats = newSeatNumber;
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
        return true;
    }


}