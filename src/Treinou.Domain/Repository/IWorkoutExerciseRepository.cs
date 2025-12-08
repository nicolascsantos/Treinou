using Treinou.Domain.Entities;
using Treinou.Domain.SeedWork;

namespace Treinou.Domain.Repository
{
    /// <summary>
    /// Repository for WorkoutExercise entity.
    /// Note: WorkoutExercise is not an AggregateRoot, so this repository
    /// provides specific operations instead of extending IGenericRepository.
    /// </summary>
    public interface IWorkoutExerciseRepository : IRepository
    {
        Task Insert(WorkoutExercise workoutExercise, CancellationToken cancellationToken);

        Task<WorkoutExercise> Get(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<WorkoutExercise>> GetByWorkoutId(Guid workoutId, CancellationToken cancellationToken);

        Task Update(WorkoutExercise workoutExercise, CancellationToken cancellationToken);

        Task Delete(WorkoutExercise workoutExercise, CancellationToken cancellationToken);
    }
}
