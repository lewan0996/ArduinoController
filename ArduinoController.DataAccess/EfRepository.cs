using System;
using System.Linq;
using System.Linq.Expressions;
using ArduinoController.Core.Contract.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ArduinoController.DataAccess
{
    public class EfRepository<T> : IRepository<T> where T : class
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

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] propertiesToLoad)
        {
            if (propertiesToLoad == null) return _set;
            var result = _set.AsQueryable();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var expression in propertiesToLoad)
            {
                result = result.Include(expression);
            }

            return result;
        }
    }
}
