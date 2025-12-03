namespace Treinou.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        public Task Commit(CancellationToken cancellationToken);
        public Task Rollback(CancellationToken cancellationToken);
    }
}
