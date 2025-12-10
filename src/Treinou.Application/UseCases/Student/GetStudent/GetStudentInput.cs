using MediatR;
using Treinou.Application.UseCases.Student.Common;

namespace Treinou.Application.UseCases.Student.GetStudent
{
    public class GetStudentInput : IRequest<StudentModelOutput>
    {
        public Guid Id { get; set; }

        public GetStudentInput(Guid id)
            => Id = id;
    }
}
