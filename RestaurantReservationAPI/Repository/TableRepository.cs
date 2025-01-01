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

    public async Task<bool> UpdateTableSeats(Guid tableId, int newSeatNumber)
    {
        var table = await _context.Tables.FindAsync(tableId);
        if (table == null) return false;

        table.Seats = newSeatNumber;
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
        return true;
    }


}