using RestaurantReservationAPI.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Interface;

namespace RestaurantReservationAPI.Repository;
public class BaseRepository<T>(DataContext context) : IBaseRepository<T>
  where T : class
{
  private readonly DbSet<T> _dbSet = context.Set<T>();

  public async Task<bool> DoesEntityExistAsync(Guid id)
  {
    var entity = await _dbSet.FindAsync(id);
    return entity != null;
  }

  public async Task<ICollection<T>> GetAllAsync()
  {
    return await _dbSet.ToListAsync();
  }

  public async Task<T?> CreateAsync(T newEntity)
  {
    await _dbSet.AddAsync(newEntity);
    await context.SaveChangesAsync();
    return newEntity;
  }

  public async Task<T?> GetByIdAsync(Guid id)
  {
    return await _dbSet.FindAsync(id);
  }

  public async Task<T?> UpdateEntityByIdAsync(T updateEntity)
  {
    _dbSet.Update(updateEntity);
    await context.SaveChangesAsync();
    return updateEntity;
  }

  public async Task<bool> DeleteByIdAsync(Guid id)
  {
    var entity = await _dbSet.FindAsync(id);
    if (entity == null) return false;

    _dbSet.Remove(entity);
    await context.SaveChangesAsync();
    return true;
  }

  public async Task<int> SaveChangesAsync()
  {
    return await context.SaveChangesAsync();
  }
}