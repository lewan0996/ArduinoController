using ArduinoController.Core.Models;

namespace ArduinoController.Core.Contract.Services
{
    public interface IProcedureService
    {
        Procedure Add(Procedure procedure);
        void Delete(int id);
        Procedure Update(int id, Procedure newProcedure);
    }
}
