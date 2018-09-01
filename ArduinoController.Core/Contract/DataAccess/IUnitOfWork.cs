using System;

namespace ArduinoController.Core.Contract.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWork Create();
        void Commit();
    }
}
