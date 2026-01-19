using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.WorkoutExercise.ListWorkoutExercises
{
    public class ListWorkoutExercises : IListWorkoutExercises
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;

        public ListWorkoutExercises(IWorkoutExerciseRepository workoutExerciseRepository)
            => _workoutExerciseRepository = workoutExerciseRepository;

        public async Task<ListWorkoutExercisesOutput> Handle(ListWorkoutExerciseInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _workoutExerciseRepository.Search(request.ToSearchInput(), cancellationToken);

            var output = ListWorkoutExercisesOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
