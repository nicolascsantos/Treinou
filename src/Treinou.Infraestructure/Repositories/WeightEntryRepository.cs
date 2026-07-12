using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class WeightEntryRepository : IWeightEntryRepository
    {
        private readonly TreinouDbContext _context;
        private DbSet<WeightEntry> _weightEntries => _context.Set<WeightEntry>();

        public WeightEntryRepository(TreinouDbContext context)
            => _context = context;
        

        public IQueryable<WeightEntry> AddOrderToQuery(IQueryable<WeightEntry> query, string propertyToOrderBy, SearchOrder order)
        => (propertyToOrderBy.ToLower(), order) switch
        {
            ("weight", SearchOrder.ASCENDING) => query.OrderBy(x => x.Weight),
            ("weight", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Weight),
            ("dateadded", SearchOrder.ASCENDING) => query.OrderBy(x => x.DateAdded),
            ("dateadded", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.DateAdded),
            _ => query.OrderBy(x => x.DateAdded)
        };

        public Task Delete(WeightEntry aggregate, CancellationToken cancellationToken)
            => Task.FromResult(_weightEntries.Remove(aggregate));
        

        public async Task<WeightEntry> Get(Guid id, CancellationToken cancellationToken)
            => await _weightEntries.FindAsync(id, cancellationToken) ?? throw new NotFoundException($"Weight entry {id} not found.");

        public async Task Insert(WeightEntry aggregate, CancellationToken cancellationToken)
            => await _weightEntries.AddAsync(aggregate, cancellationToken);

        public async Task<SearchOutput<WeightEntry>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _weightEntries.AsNoTracking();
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Weight.ToString().Contains(searchInput.Search));
            var total = await query.CountAsync();
            var weightEntries = await query.Skip(toSkip)
                .Take(searchInput.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<WeightEntry>(
                searchInput.Page,
                searchInput.PerPage,
                total,
                weightEntries
            );
        }

        public async Task Update(WeightEntry aggregate, CancellationToken cancellationToken)
            => _weightEntries.Update(aggregate);
    }
}
