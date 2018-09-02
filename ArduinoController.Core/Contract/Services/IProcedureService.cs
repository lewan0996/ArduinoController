using ArduinoController.Core.Models;

namespace ArduinoController.Core.Contract.Services
{
    public interface IProcedureService
    {
        Procedure Add(Procedure procedure);
        void Delete(string userId, string name);
        Procedure Update(Procedure newProcedure);
    }
}
