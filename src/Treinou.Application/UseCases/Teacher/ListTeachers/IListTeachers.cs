using MediatR;

namespace Treinou.Application.UseCases.Teacher.ListTeachers
{
    public interface IListTeachers : IRequestHandler<ListTeachersInput, ListTeachersOutput>
    {
    }
}
