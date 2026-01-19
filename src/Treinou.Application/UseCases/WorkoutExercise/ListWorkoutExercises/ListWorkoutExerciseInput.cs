using MediatR;
using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.WorkoutExercise.ListWorkoutExercises
{
    public class ListWorkoutExerciseInput : PaginatedListInput, IRequest<ListWorkoutExercisesOutput>
    {
        public ListWorkoutExerciseInput(
            int page,
            int perPage,
            string search,
            string sort,
            SearchOrder dir
        ) : base(page, perPage, search, sort, dir) {}

        public ListWorkoutExerciseInput() : base(1, 15, "", "", SearchOrder.ASCENDING) {}
    }
}
