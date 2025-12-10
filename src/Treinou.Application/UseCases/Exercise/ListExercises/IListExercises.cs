using MediatR;

namespace Treinou.Application.UseCases.Exercise.ListExercises
{
    public interface IListExercises : IRequestHandler<ListExercisesInput, ListExercisesOutput>
    {
    }
}
