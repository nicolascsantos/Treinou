using Treinou.Application.Common;
using Treinou.Application.UseCases.Exercise.Common;
using Treinou.Domain.SeedWork.SearchableRepository;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.Exercise.ListExercises
{
    public class ListExercisesOutput : PaginatedListOutput<ExerciseModelOutput>
    {
        public ListExercisesOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<ExerciseModelOutput> items
        ) : base(page, perPage, total, items)
        {
        }

        public static ListExercisesOutput FromSearchOutput(
           SearchOutput<Entity.Exercise> searchOutput)
        {
            return new ListExercisesOutput(
                page: searchOutput.Page,
                perPage: searchOutput.PerPage,
                total: searchOutput.Total,
                items: searchOutput.Items
                .Select(ExerciseModelOutput.FromExercise).ToList()
            );
        }
    }
}
