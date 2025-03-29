using System.Linq.Expressions;

namespace POS_API.Repository.IRepository
{
    public interface IGenericRepository
    {
        public interface IGenericRepository<T> where T : class
        {
            Task<T?> GetByIdAsync(object id);
            Task<IEnumerable<T>> GetAllAsync();
            Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
            Task AddAsync(T entity);
            Task AddRangeAsync(IEnumerable<T> entities);
            bool Update(T entity);
            bool Remove(T entity);
            void RemoveRange(IEnumerable<T> entities);
        }
    }
}
