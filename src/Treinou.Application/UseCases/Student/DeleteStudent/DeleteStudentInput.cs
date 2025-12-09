using MediatR;

namespace Treinou.Application.UseCases.Student.DeleteStudent
{
    public class DeleteStudentInput : IRequest
    {
        public Guid Id { get; set; }

        public DeleteStudentInput(Guid id)
        {
            Id = id;
        }
    }
}
