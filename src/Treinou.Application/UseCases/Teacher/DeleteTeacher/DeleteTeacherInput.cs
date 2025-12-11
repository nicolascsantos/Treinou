using MediatR;

namespace Treinou.Application.UseCases.Teacher.DeleteTeacher
{
    public class DeleteTeacherInput : IRequest<Unit>
    {
        public Guid Id { get; private set; }

        public DeleteTeacherInput(Guid id)
            => Id = id;
    }
}
