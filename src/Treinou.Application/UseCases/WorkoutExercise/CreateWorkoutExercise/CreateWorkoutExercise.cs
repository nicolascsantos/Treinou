using Treinou.Application.UseCases.WorkoutExercise.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.WorkoutExercise.CreateWorkoutExercise
{
    public class CreateWorkoutExercise : ICreateWorkoutExercise
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWorkoutExercise(IWorkoutExerciseRepository workoutExerciseRepository, IUnitOfWork unitOfWork)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkoutExerciseModelOutput> Handle(CreateWorkoutExerciseInput request, CancellationToken cancellationToken)
        {
            var workoutExercise = new Entity.WorkoutExercise(
                request.WorkoutId,
                request.ExerciseId,
                request.Order,
                request.NumberOfSets,
                request.NumberOfRepetitions,
                request.Rest,
                request.Notes
            );

            await _workoutExerciseRepository.Insert(workoutExercise, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return WorkoutExerciseModelOutput.FromWorkoutExercise(workoutExercise);
        }
    }
}
