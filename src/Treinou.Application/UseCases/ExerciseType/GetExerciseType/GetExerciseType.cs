using Treinou.Application.UseCases.ExerciseType.Common;
using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.ExerciseType.GetExerciseType
{
    public class GetExerciseType : IGetExerciseType
    {
        private readonly IExerciseTypeRepository _exerciseTypeRepository;

        public GetExerciseType(IExerciseTypeRepository exerciseTypeRepository)
            => _exerciseTypeRepository = exerciseTypeRepository;

        public async Task<ExerciseTypeModelOutput> Handle(GetExerciseTypeInput request, CancellationToken cancellationToken)
        {
            var exerciseType = await _exerciseTypeRepository.Get(request.Id, cancellationToken);

            return ExerciseTypeModelOutput.FromExerciseType(exerciseType);
        }
    }
}
