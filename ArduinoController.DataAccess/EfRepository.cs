using System.Linq;
using ArduinoController.Core.Contract.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ArduinoController.DataAccess
{
    public class EfRepository<T> : IRepository<T> where T: class
    {
        private readonly DbSet<T> _set;
        // ReSharper disable once SuggestBaseTypeForParameter
        public EfRepository(AppDbContext context)
        {
            _set = context.Set<T>();
        }
        public T Add(T entity)
        {
            return _set.Add(entity).Entity;
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public T Get(params object[] id)
        {
            return _set.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _set;
        }
    }
}
