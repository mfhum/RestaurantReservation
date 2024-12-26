using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Repository;

public class TableRepository(DataContext context) : BaseRepository<Table>(context), ITableRepository
{
  private DataContext Context { get; } = context;
  public async Task<bool> UpdateTableSeats(Guid tableId, int newSeatNumber)
  {
    var table = await Context.Tables.FindAsync(tableId);
    if (table == null) return false;

    table.Seats = newSeatNumber;
    Context.Tables.Update(table);
    await Context.SaveChangesAsync();
    return true;
  }
}