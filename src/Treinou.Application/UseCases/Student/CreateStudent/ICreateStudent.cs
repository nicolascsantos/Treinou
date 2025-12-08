using MediatR;
using Treinou.Application.UseCases.Student.Common;

namespace Treinou.Application.UseCases.Student.CreateStudent
{
    public interface ICreateStudent : IRequestHandler<CreateStudentInput, StudentModelOutput>
    {
    }
}
