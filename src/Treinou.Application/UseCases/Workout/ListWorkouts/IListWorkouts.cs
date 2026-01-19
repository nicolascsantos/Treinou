using MediatR;

namespace Treinou.Application.UseCases.Workout.ListWorkouts
{
    public interface IListWorkouts : IRequestHandler<ListWorkoutsInput, ListWorkoutsOutput>
    {
    }
}
