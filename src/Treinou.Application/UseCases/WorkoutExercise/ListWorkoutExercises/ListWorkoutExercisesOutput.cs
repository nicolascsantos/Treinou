using Treinou.Application.Common;
using Treinou.Application.UseCases.WorkoutExercise.Common;
using Treinou.Domain.SeedWork.SearchableRepository;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.WorkoutExercise.ListWorkoutExercises
{
    public class ListWorkoutExercisesOutput : PaginatedListOutput<WorkoutExerciseModelOutput>
    {
        public ListWorkoutExercisesOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<WorkoutExerciseModelOutput> items
        ) : base(page, perPage, total, items)
        {
            
        }

        public static ListWorkoutExercisesOutput FromSearchOutput(SearchOutput<Entity.WorkoutExercise> searchOutput)
            => new(
                searchOutput.Page,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items.Select(WorkoutExerciseModelOutput.FromWorkoutExercise).ToList()
            );
    }
}
