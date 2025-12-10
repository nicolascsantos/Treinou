using MediatR;
using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.ExerciseType.ListExerciseTypes
{
    public class ListExerciseTypesInput : PaginatedListInput, IRequest<ListExerciseTypesOutput>
    {
        public ListExerciseTypesInput(
            int page = 1,
            int perPage = 15,
            string search = "",
            string sort = "",
            SearchOrder order = SearchOrder.ASCENDING
        ) : base(page, perPage, search, sort, order)
        {}

        public ListExerciseTypesInput()
           : base(1, 15, "", "", SearchOrder.ASCENDING)
        { }
    }
}
