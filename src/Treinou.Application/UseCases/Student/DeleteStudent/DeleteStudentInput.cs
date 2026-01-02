using MediatR;

namespace Treinou.Application.UseCases.Student.DeleteStudent
{
    public class DeleteStudentInput : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteStudentInput(Guid id)
        {
            Id = id;
        }
    }
}
