using MediatR;

namespace Treinou.Application.UseCases.Teacher.DeleteTeacher
{
    public interface IDeleteTeacher : IRequestHandler<DeleteTeacherInput, Unit>
    {
    }
}
