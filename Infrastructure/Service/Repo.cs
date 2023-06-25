using Microsoft.EntityFrameworkCore;
using Domain.Interface;
using Infrastructure.Persistence;
using Domain.Model;
using Infrastructure.Interface;

namespace Infrastructure.Service
{
    public class Repo<T> : IRepo<T> where T: class, IEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repo(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IEntity>> GetAllAsync(bool shallow)
        {
            IQueryable<IEntity> query = _dbSet;

            if (shallow)
            {
                query = query.Select(p => new BaseEntity { Id = p.Id, Name = p.Name });
            }

            return await query.ToAsyncEnumerable().ToListAsync();
        }



    }
}
