using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Student.DeleteStudent
{
    public class DeleteStudent : IDeleteStudent
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudent(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork
        )
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            DeleteStudentInput request,
            CancellationToken cancellationToken
        )
        {
            // Get the student to ensure it exists
            var student = await _studentRepository.Get(request.Id, cancellationToken);

            // Delete the student
            await _studentRepository.Delete(student, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
