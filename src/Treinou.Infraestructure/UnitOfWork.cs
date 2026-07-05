using Microsoft.EntityFrameworkCore.Storage;
using Treinou.Domain.SeedWork;

namespace Treinou.Infraestructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TreinouDbContext _context;

        public IDbContextTransaction? _transaction;

        public UnitOfWork(TreinouDbContext context)
            => _context = context;

        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();


        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.SaveChangesAsync(cancellationToken);

                if (_transaction != null)
                    await _transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch (Exception)
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
            finally 
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            _context.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            _context.DisposeAsync();
            _transaction?.DisposeAsync();
        }
    }
}
