using System;

namespace ArduinoController.Core.Contract.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
