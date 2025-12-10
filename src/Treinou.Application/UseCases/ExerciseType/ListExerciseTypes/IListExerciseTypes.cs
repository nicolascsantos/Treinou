using MediatR;

namespace Treinou.Application.UseCases.ExerciseType.ListExerciseTypes
{
    public interface IListExerciseTypes : IRequestHandler<ListExerciseTypesInput, ListExerciseTypesOutput>
    {
    }
}
