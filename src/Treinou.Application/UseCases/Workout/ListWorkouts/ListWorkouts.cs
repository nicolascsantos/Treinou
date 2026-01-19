using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Workout.ListWorkouts
{
    public class ListWorkouts : IListWorkouts
    {
        private readonly IWorkoutRepository _workoutRepository;

        public ListWorkouts(IWorkoutRepository workoutRepository)
            => _workoutRepository = workoutRepository;

        public async Task<ListWorkoutsOutput> Handle(ListWorkoutsInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _workoutRepository.Search(request.ToSearchInput(), cancellationToken);

            var output = ListWorkoutsOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
