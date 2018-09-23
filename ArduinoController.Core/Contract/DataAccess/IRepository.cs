using System;
using System.Linq;
using System.Linq.Expressions;

namespace ArduinoController.Core.Contract.DataAccess
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Delete(T entity);
        T Get(params object[] id);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] propertiesToLoad);
    }
}
