using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Find(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<List<T>> Where(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}