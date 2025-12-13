using Treinou.Application.UseCases.WorkoutExercise.Common;
using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.WorkoutExercise.GetWorkoutExercise
{
    public class GetWorkoutExercise : IGetWorkoutExercise
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;

        public GetWorkoutExercise(IWorkoutExerciseRepository workoutExerciseRepository)
            => _workoutExerciseRepository = workoutExerciseRepository;

        public async Task<WorkoutExerciseModelOutput> Handle(GetWorkoutExerciseInput request, CancellationToken cancellationToken)
        {
            var workoutExercise = await _workoutExerciseRepository.Get(request.Id, cancellationToken);

            return WorkoutExerciseModelOutput.FromWorkoutExercise(workoutExercise);
        }
    }
}
