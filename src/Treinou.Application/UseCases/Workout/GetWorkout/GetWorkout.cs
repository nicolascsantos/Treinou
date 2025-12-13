using Treinou.Application.UseCases.Workout.Common;
using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Workout.GetWorkout
{
    public class GetWorkout : IGetWorkout
    {
        private readonly IWorkoutRepository _workoutRepository;

        public GetWorkout(IWorkoutRepository workoutRepository)
        => _workoutRepository = workoutRepository;

        public async Task<WorkoutModelOutput> Handle(GetWorkoutInput request, CancellationToken cancellationToken)
        {
            var workout = await _workoutRepository.Get(request.Id, cancellationToken);

            return WorkoutModelOutput.FromWorkout(workout);
        }
    }
}
