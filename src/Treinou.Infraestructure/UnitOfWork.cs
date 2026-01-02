using Treinou.Domain.SeedWork;

namespace Treinou.Infraestructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TreinouDbContext _context;

        public UnitOfWork(TreinouDbContext context)
            => _context = context;

        public async Task Commit(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken);

        public Task Rollback(CancellationToken cancellationToken)
            => Task.CompletedTask;  
    }
}
