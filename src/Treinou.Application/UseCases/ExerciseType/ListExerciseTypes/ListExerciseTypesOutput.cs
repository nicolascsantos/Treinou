using Treinou.Application.Common;
using Treinou.Application.UseCases.ExerciseType.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.ExerciseType.ListExerciseTypes
{
    public class ListExerciseTypesOutput : PaginatedListOutput<ExerciseTypeModelOutput>
    {
        public ListExerciseTypesOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<ExerciseTypeModelOutput> items
        ) : base(page, perPage, total, items)
        { }

        public static ListExerciseTypesOutput FromSearchOutput(SearchOutput<ExerciseTypeModelOutput> searchOutput)
            => new ListExerciseTypesOutput(
                searchOutput.Page,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items
            );
    }
}
