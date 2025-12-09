using MediatR;

namespace Treinou.Application.UseCases.Student.DeleteStudent
{
    public interface IDeleteStudent : IRequestHandler<DeleteStudentInput>
    {
    }
}
