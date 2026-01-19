using MediatR;
using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.Workout.ListWorkouts
{
    public class ListWorkoutsInput : PaginatedListInput, IRequest<ListWorkoutsOutput>
    {
        public ListWorkoutsInput(
            int page,
            int perPage,
            string search,
            string sort,
            SearchOrder dir
        ) : base(page, perPage, search, sort, dir) {}

        public ListWorkoutsInput() : base(1, 15, "", "", SearchOrder.ASCENDING) { }
    }
}
