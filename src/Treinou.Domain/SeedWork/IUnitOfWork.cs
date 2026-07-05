namespace Treinou.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        public Task BeginTransactionAsync();
        public Task<int> CommitAsync(CancellationToken cancellationToken);
        public Task RollbackAsync(CancellationToken cancellationToken);
    }
}
