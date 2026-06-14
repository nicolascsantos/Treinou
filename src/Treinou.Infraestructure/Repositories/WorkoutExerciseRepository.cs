using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class WorkoutExerciseRepository : IWorkoutExerciseRepository
    {
        private readonly TreinouDbContext _context;

        private DbSet<WorkoutExercise> _workoutExercises => _context.Set<WorkoutExercise>();

        public WorkoutExerciseRepository(TreinouDbContext context)
            => _context = context;

        public async Task Insert(WorkoutExercise workoutExercise, CancellationToken cancellationToken)
            => await _workoutExercises.AddAsync(workoutExercise, cancellationToken);

        public async Task<WorkoutExercise> Get(Guid id, CancellationToken cancellationToken)
        {
            var workoutExercise = await _workoutExercises
                .Include(we => we.Exercise)
                .FirstOrDefaultAsync(we => we.Id == id, cancellationToken);

            if (workoutExercise is null)
                throw new NotFoundException($"WorkoutExercise '{id}' not found");

            return workoutExercise;
        }

        public async Task<IEnumerable<WorkoutExercise>> GetByWorkoutId(Guid workoutId, CancellationToken cancellationToken)
        {
            return await _workoutExercises
                .Include(we => we.Exercise)
                .Where(we => we.WorkoutId == workoutId)
                .OrderBy(we => we.Order)
                .ToListAsync(cancellationToken);
        }

        public async Task Update(WorkoutExercise workoutExercise, CancellationToken cancellationToken)
            => await Task.FromResult(_context.Update(workoutExercise));

        public Task Delete(WorkoutExercise workoutExercise, CancellationToken cancellationToken)
        {
            _workoutExercises.Remove(workoutExercise);
            return Task.CompletedTask;
        }

        public async Task<SearchOutput<WorkoutExercise>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _workoutExercises.AsNoTracking().AsQueryable();
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Workout.Name.Contains(searchInput.Search));
            var total = await query.CountAsync();
            var workoutExercises = await query.Skip(toSkip)
                .Take(searchInput.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<WorkoutExercise>(
                searchInput.Page,
                searchInput.PerPage,
                total,
                workoutExercises
            );
        }

        public IQueryable<WorkoutExercise> AddOrderToQuery(IQueryable<WorkoutExercise> query, string propertyToOrderBy, SearchOrder order)
            => (propertyToOrderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.ASCENDING) => query.OrderBy(x => x.Workout.Name),
                ("name", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Workout.Name),
                _ => query.OrderBy(x => x.Id)
            };
    }
}
