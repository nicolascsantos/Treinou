using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.ExerciseType.ListExerciseTypes
{
    public class ListExerciseTypesOutput : PaginatedListOutput<Entity.ExerciseType>
    {
        public ListExerciseTypesOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<Entity.ExerciseType> items
        ) : base(page, perPage, total, items)
        {}

        public static ListExerciseTypesOutput FromSearchOutput(SearchOutput<Entity.ExerciseType> searchOutput)
            => new ListExerciseTypesOutput(
                searchOutput.Page,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items
            );
    }
}
