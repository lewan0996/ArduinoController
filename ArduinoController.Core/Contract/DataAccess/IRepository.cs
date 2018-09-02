using System.Linq;

namespace ArduinoController.Core.Contract.DataAccess
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Delete(T entity);
        T Get(params object[] id);
        IQueryable<T> GetAll();
    }
}
