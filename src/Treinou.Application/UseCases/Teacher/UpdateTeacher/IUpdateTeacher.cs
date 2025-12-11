using MediatR;
using Treinou.Application.UseCases.Teacher.Common;

namespace Treinou.Application.UseCases.Teacher.UpdateTeacher
{
    public  interface IUpdateTeacher : IRequestHandler<UpdateTeacherInput, TeacherModelOutput>
    {
    }
}
