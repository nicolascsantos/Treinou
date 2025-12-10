using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.ExerciseType.ListExerciseTypes
{
    public class ListExerciseTypes : IListExerciseTypes
    {
        private readonly IExerciseTypeRepository _exerciseTypeRepository;

        public ListExerciseTypes(IExerciseTypeRepository exerciseTypeRepository)
            => _exerciseTypeRepository = exerciseTypeRepository;


        public async Task<ListExerciseTypesOutput> Handle(ListExerciseTypesInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _exerciseTypeRepository.Search(request.ToSearchInput(), cancellationToken);

            var output = ListExerciseTypesOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
