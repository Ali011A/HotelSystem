using Hotel.Domain.Interfaces.Repositories;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IReservationRepository Reservations { get; private set; }
        private IDbContextTransaction? _currentTransaction;
        private int _transactionDepth = 0;
        private readonly object _transactionLock = new object();
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Reservations = new ReservationRepository(_context);
        }
//        public void Dispose()
//        {
//_context.Dispose();
//        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
          var r=    await _context.SaveChangesAsync(cancellationToken);
            return r;
        }
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            lock (_transactionLock)
            {
                if (_currentTransaction == null)
                {
                    // create below outside lock
                    _transactionDepth = 0;
                }
                _transactionDepth++;
            }

            
            if (_currentTransaction != null) return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

       
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            
            lock (_transactionLock)
            {
                if (_transactionDepth <= 0)
                {
                    // nothing to commit
                    return;
                }
                _transactionDepth--;
            }

            // only commit w
            if (_transactionDepth > 0) return;

            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                // ensure changes are saved before commit
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

      
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            lock (_transactionLock)
            {
                _transactionDepth = 0;
            }

            if (_currentTransaction == null) return;

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        
        private bool _disposed;
        public void Dispose()
        {
            if (_disposed) return;

            if (_currentTransaction != null)
            {
                try
                {
                    _currentTransaction.Rollback();
                    _currentTransaction.Dispose();
                }
                catch
                {
                    
                }
                _currentTransaction = null;
            }

            _context.Dispose();
            _disposed = true;
        }
    }
}
