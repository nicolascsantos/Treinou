using MediatR;

namespace Treinou.Application.UseCases.Student.ListStudent
{
    public interface IListStudents : IRequestHandler<ListStudentsInput, ListStudentsOutput>
    {
    }
}
