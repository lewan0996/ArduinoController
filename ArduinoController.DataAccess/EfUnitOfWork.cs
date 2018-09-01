using ArduinoController.Core.Contract.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace ArduinoController.DataAccess
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _shouldRollback;
        private bool _disposed;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;
            _shouldRollback = true;
        }
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_shouldRollback)
            {
                _transaction.Rollback();
            }
            else
            {
                _transaction.Commit();
            }

            _transaction.Dispose();
            _context.Dispose();

            _disposed = true;
        }

        public IUnitOfWork Create()
        {
           _transaction = _context.Database.BeginTransaction();
            return this;
        }

        public void Commit()
        {
            _shouldRollback = true;
            _context.SaveChanges();
            _shouldRollback = false;
        }
    }
}
