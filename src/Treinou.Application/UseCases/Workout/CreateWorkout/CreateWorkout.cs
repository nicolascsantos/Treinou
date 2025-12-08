using Treinou.Application.UseCases.Workout.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Workout.CreateWorkout
{
    public class CreateWorkout : ICreateWorkout
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWorkout(
            IWorkoutRepository workoutRepository,
            IUnitOfWork unitOfWork
        )
        {
            _workoutRepository = workoutRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkoutModelOutput> Handle(
            CreateWorkoutInput request,
            CancellationToken cancellationToken
        )
        {
            var workout = new Domain.Entities.Workout(
                request.Name,
                request.TeacherId,
                request.StudentId,
                request.IsActive
            );

            await _workoutRepository.Insert(workout, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return WorkoutModelOutput.FromWorkout(workout);
        }
    }
}
