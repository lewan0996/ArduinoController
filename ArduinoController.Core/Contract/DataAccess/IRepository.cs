using System.ComponentModel;
using ArduinoController.Core.Models;

namespace ArduinoController.Core.Contract.DataAccess
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Delete(T entity);
        T Get(int id);
    }
}
