using Microsoft.EntityFrameworkCore;
using POS_API.DatabaseContext;
using static POS_API.Repository.IRepository.IGenericRepository;
using System.Linq.Expressions;

namespace POS_API.Repository.Imp
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly POSSystemDBContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(POSSystemDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public bool Update(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }

        public bool Remove(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
