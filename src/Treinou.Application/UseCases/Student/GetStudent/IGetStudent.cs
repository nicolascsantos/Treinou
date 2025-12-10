using MediatR;
using Treinou.Application.UseCases.Student.Common;

namespace Treinou.Application.UseCases.Student.GetStudent
{
    public interface IGetStudent : IRequestHandler<GetStudentInput, StudentModelOutput>
    {
    }
}
