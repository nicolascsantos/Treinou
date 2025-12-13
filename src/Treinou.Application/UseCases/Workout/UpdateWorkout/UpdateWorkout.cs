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

            AddExercises(request, workoutToBeUpdated);

            await _workoutRepository.Update(workoutToBeUpdated, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return WorkoutModelOutput.FromWorkout(workoutToBeUpdated);
        }

        private static void AddExercises(UpdateWorkoutInput request, Domain.Entities.Workout workoutToBeUpdated)
        {
            if (request.Exercises.Count > 0)
            {
                request.Exercises.ForEach(exercise => workoutToBeUpdated.AddExercise(
                    exercise.Id,
                    exercise.Exercise,
                    exercise.Order,
                    exercise.NumberOfSets,
                    exercise.NumberOfRepetitions,
                    exercise.Rest,
                    exercise.Notes
                ));
            }
        }
    }
}
