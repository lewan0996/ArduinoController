using ArduinoController.Core.Contract.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace ArduinoController.DataAccess
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IDbContextTransaction _transaction;
        private bool _shouldRollback;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;
            _transaction = context.Database.BeginTransaction();
            _shouldRollback = true;
        }
        public void Dispose()
        {
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
        }

        public void Commit()
        {
            _shouldRollback = true;
            _context.SaveChanges();
            _shouldRollback = false;
        }
    }
}
