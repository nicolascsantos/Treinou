using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;

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
    }
}
