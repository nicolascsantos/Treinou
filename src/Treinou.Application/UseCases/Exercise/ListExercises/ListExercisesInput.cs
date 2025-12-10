using MediatR;
using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.Exercise.ListExercises
{
    public class ListExercisesInput : PaginatedListInput, IRequest<ListExercisesOutput>
    {
        public ListExercisesInput(
            int page = 1,
            int perPage = 15,
            string search = "",
            string sort = "",
            SearchOrder dir = SearchOrder.ASCENDING
        ) : base(page, perPage, search, sort, dir)
        {
        }

        public ListExercisesInput() : base(1, 15, "", "", SearchOrder.ASCENDING)
        {}
    }
}
