using MediatR;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Teacher.DeleteTeacher
{
    public class DeleteTeacher : IDeleteTeacher
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeacher(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
            => (_teacherRepository, _unitOfWork) = (teacherRepository, unitOfWork);

        public async Task<Unit> Handle(DeleteTeacherInput request, CancellationToken cancellationToken)
        {
            var teacherToBeDeleted = await _teacherRepository.Get(request.Id, cancellationToken);

            await _teacherRepository.Delete(teacherToBeDeleted, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
