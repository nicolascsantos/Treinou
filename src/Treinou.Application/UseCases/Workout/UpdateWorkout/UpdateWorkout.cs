using Treinou.Application.UseCases.Workout.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Workout.UpdateWorkout
{
    public class UpdateWorkout : IUpdateWorkout
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkout(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
            => (_workoutRepository, _unitOfWork) = (workoutRepository, unitOfWork);


        public async Task<WorkoutModelOutput> Handle(UpdateWorkoutInput request, CancellationToken cancellationToken)
        {
            var workoutToBeUpdated = await _workoutRepository
                .Get(request.Id, cancellationToken);

            workoutToBeUpdated.Update(
                request.Name,
                request.TeacherId,
                request.StudentId
            );

            await _workoutRepository.Update(workoutToBeUpdated, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return WorkoutModelOutput.FromWorkout(workoutToBeUpdated);
        }
    }
}
