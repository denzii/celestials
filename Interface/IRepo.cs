namespace Celestials.Interface
{
    public interface IRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<IEntity>> GetAllAsync(bool shallow=false);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
    }
}
