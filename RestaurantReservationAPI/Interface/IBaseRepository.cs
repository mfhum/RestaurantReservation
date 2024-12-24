namespace RestaurantReservationAPI.Interface;

public interface IBaseRepository<T> where T: class
{
  // CRUD Operations
  Task<bool> DoesEntityExistAsync(Guid id);
  Task<ICollection<T>> GetAllAsync();
  Task<T?> CreateAsync(T newEntity);
  Task<T?> GetByIdAsync(Guid id);
  Task<T?> UpdateEntityByIdAsync(T updateEntity);
  Task<bool> DeleteByIdAsync(Guid id);
  Task<int> SaveChangesAsync();
}