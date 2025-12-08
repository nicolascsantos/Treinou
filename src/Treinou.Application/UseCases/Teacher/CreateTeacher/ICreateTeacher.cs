using MediatR;
using Treinou.Application.UseCases.Teacher.Common;

namespace Treinou.Application.UseCases.Teacher.CreateTeacher
{
    public interface ICreateTeacher : IRequestHandler<CreateTeacherInput, TeacherModelOutput>
    {
    }
}
