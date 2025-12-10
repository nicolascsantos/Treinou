using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Domain.SeedWork
{
    public interface IGenericRepository<TAggregate> : IRepository where TAggregate : AggregateRoot
    {
        public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);

        public Task<TAggregate> Get(Guid id, CancellationToken cancellationToken);

        public Task Delete(TAggregate aggregate, CancellationToken cancellationToken);

        public Task Update(TAggregate aggregate, CancellationToken cancellationToken);
        IQueryable<TAggregate> AddOrderToQuery(IQueryable<TAggregate> query, string propertyToOrderBy, SearchOrder order);
    }
}
