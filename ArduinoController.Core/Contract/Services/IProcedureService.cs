using System.Linq;
using ArduinoController.Core.Models;

namespace ArduinoController.Core.Contract.Services
{
    public interface IProcedureService
    {
        void Add(Procedure procedure);
        void Delete(int id);
        void Update(int id, Procedure newProcedure);
        IQueryable<Procedure> GetUserProcedures(string userId);
    }
}
