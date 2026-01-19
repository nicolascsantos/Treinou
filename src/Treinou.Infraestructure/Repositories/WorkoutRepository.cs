using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly TreinouDbContext _context;
        private DbSet<Workout> _workouts 
            => _context.Set<Workout>();

        public WorkoutRepository(TreinouDbContext context) 
            => _context = context;


        public Task Delete(Workout aggregate, CancellationToken cancellationToken)
        {
            _workouts.Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<Workout> Get(Guid id, CancellationToken cancellationToken)
        {
            var workout = await _workouts.FindAsync(id);
            if (workout is null)
                throw new NotFoundException($"Workout '{id}' not found.");
            return workout;
        }

        public async Task Insert(Workout aggregate, CancellationToken cancellationToken)
            => await _context.AddAsync(aggregate);

        public async Task Update(Workout aggregate, CancellationToken cancellationToken)
            => await Task.FromResult(_context.Update(aggregate));

        public IQueryable<Workout> AddOrderToQuery(IQueryable<Workout> query, string propertyToOrderBy, SearchOrder order)
            => (propertyToOrderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.ASCENDING) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };


        public async Task<SearchOutput<Workout>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _workouts.AsNoTracking().AsQueryable();
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Name.Contains(searchInput.Search));
            var total = await query.CountAsync();
            var workouts = await query.Skip(toSkip)
                .Take(searchInput.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<Workout>(
                searchInput.Page,
                searchInput.PerPage,
                total,
                workouts
            );
        }
    }
}
