using System.Linq;
using System.Threading.Tasks;
using ArduinoController.Core.Models;

namespace ArduinoController.Core.Contract.Services
{
    public interface IProcedureService
    {
        void Add(Procedure procedure);
        void Delete(int id);
        void Update(int id, Procedure newProcedure);
        IQueryable<Procedure> GetUserProcedures(string userId);
        IQueryable<Procedure> GetAllProcedures();
        Task ExecuteAsync(Procedure procedure);
    }
}
