using MediatR;
using Treinou.Application.UseCases.Teacher.Common;

namespace Treinou.Application.UseCases.Teacher.GetTeacher
{
    public interface IGetTeacher : IRequestHandler<GetTeacherInput, TeacherModelOutput>
    {
    }
}
