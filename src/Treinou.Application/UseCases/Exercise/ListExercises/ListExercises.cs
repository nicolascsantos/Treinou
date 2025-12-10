using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Exercise.ListExercises
{
    public class ListExercises : IListExercises
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ListExercises(IExerciseRepository exerciseRepository)
            => _exerciseRepository = exerciseRepository;


        public async Task<ListExercisesOutput> Handle(ListExercisesInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _exerciseRepository.Search(request.ToSearchInput(), cancellationToken);

            var output = ListExercisesOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
