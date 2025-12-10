using MediatR;
using Treinou.Application.UseCases.Student.Common;

namespace Treinou.Application.UseCases.Student.UpdateStudent
{
    public interface IUpdateStudent : IRequestHandler<UpdateStudentInput, StudentModelOutput>
    {
    }
}
