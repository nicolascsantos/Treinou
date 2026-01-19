using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly TreinouDbContext _context;
        private DbSet<Exercise> _exercises => _context.Set<Exercise>();

        public ExerciseRepository(TreinouDbContext context)
            => _context = context;
        

        public Task Delete(Exercise aggregate, CancellationToken cancellationToken)
        {
            _exercises.Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<Exercise> Get(Guid id, CancellationToken cancellationToken)
        {
            var exercise = await _exercises
                .Include(x => x.ExcerciseType)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (exercise is null)
                throw new NotFoundException($"Exercise '{id}' not found.");

            return exercise;
        }

        public async Task Insert(Exercise aggregate, CancellationToken cancellationToken)
            => await _exercises.AddAsync(aggregate);


        public async Task Update(Exercise aggregate, CancellationToken cancellationToken)
            => await Task.FromResult(_context.Exercises.Update(aggregate));
        public async Task<SearchOutput<Exercise>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _exercises.AsNoTracking().Include(x => x.ExcerciseType).AsQueryable();
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Name.Contains(searchInput.Search));
            var total = await query.CountAsync();
            var exercises = await query.Skip(toSkip)
                .Take(searchInput.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<Exercise>(
                searchInput.Page,
                searchInput.PerPage,
                total,
                exercises
            );
        }

        public IQueryable<Exercise> AddOrderToQuery(IQueryable<Exercise> query, string propertyToOrderBy, SearchOrder order)
        => (propertyToOrderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.ASCENDING) => query.OrderBy(x => x.Name),
            ("name", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Name),
            _ => query.OrderBy(x => x.Name)
        };
    }
}
