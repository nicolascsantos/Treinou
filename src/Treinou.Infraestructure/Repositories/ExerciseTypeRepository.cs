using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;

namespace Treinou.Infraestructure.Repositories
{
    public class ExerciseTypeRepository : IExerciseTypeRepository
    {
        private readonly TreinouDbContext _context;

        private DbSet<ExerciseType> _exerciseTypes => _context.Set<ExerciseType>();

        public ExerciseTypeRepository(TreinouDbContext context)    
            => _context = context;
        

        public Task Delete(ExerciseType aggregate, CancellationToken cancellationToken)
        {
            _exerciseTypes.Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<ExerciseType> Get(Guid id, CancellationToken cancellationToken)
        {
            var exerciseType = await _exerciseTypes.FindAsync(id);
            if (exerciseType is null) 
                throw new NotFoundException($"Exercise type '{id}' not found.");
            return exerciseType;
        }

        public async Task Insert(ExerciseType aggregate, CancellationToken cancellationToken)
            => await _exerciseTypes.AddAsync(aggregate);

        public async Task Update(ExerciseType aggregate, CancellationToken cancellationToken)
            => await Task.FromResult(_context.Update(aggregate));
    }
}
