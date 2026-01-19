using Treinou.Application.Common;
using Treinou.Application.UseCases.Workout.Common;
using Treinou.Domain.SeedWork.SearchableRepository;
using Entity = Treinou.Domain.Entities;


namespace Treinou.Application.UseCases.Workout.ListWorkouts
{
    public class ListWorkoutsOutput : PaginatedListOutput<WorkoutModelOutput>
    {
        public ListWorkoutsOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<WorkoutModelOutput> items
        ) : base(page, perPage, total, items)
        {

        }

        public static ListWorkoutsOutput FromSearchOutput(SearchOutput<Entity.Workout> searchOutput)
            => new(
                searchOutput.Page,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items.Select(WorkoutModelOutput.FromWorkout).ToList()
            );
    }
}