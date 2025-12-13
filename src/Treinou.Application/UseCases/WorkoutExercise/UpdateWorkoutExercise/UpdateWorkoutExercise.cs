using Treinou.Application.UseCases.WorkoutExercise.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.WorkoutExercise.UpdateWorkoutExercise
{
    internal class UpdateWorkoutExercise : IUpdateWorkoutExercise
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkoutExercise(IWorkoutExerciseRepository workoutExerciseRepository, IUnitOfWork unitOfWork)
            => (_workoutExerciseRepository, _unitOfWork) = (workoutExerciseRepository, unitOfWork);

        public async Task<WorkoutExerciseModelOutput> Handle(UpdateWorkoutExerciseInput request, CancellationToken cancellationToken)
        {
            var workoutExerciseToBeUpdated = await _workoutExerciseRepository.Get(request.Id, cancellationToken);

            workoutExerciseToBeUpdated.Update(
                request.Order,
                request.NumberOfSets,
                request.NumberOfRepetitions,
                request.Rest,
                request.Notes
            );

            await _workoutExerciseRepository.Update(workoutExerciseToBeUpdated, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return WorkoutExerciseModelOutput.FromWorkoutExercise(workoutExerciseToBeUpdated);
        }
    }
}
