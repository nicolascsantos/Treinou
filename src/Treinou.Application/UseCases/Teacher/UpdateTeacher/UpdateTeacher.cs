using Treinou.Application.UseCases.Teacher.Common;
using Treinou.Domain.Factories;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Teacher.UpdateTeacher
{
    public class UpdateTeacher : IUpdateTeacher
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeacher(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
            => (_teacherRepository, _unitOfWork) = (teacherRepository, unitOfWork);

        public async Task<TeacherModelOutput> Handle(UpdateTeacherInput request, CancellationToken cancellationToken)
        {
            var teacherToBeUpdated = await _teacherRepository.Get(request.Id, cancellationToken);

            teacherToBeUpdated.Update(
                request.Name,
                ValueObjectFactory.CreateEmail(request.Email),
                ValueObjectFactory.CreateCPF(request.CPF),
                ValueObjectFactory.CreateCREF(request.CREF),
                request.Description,
                ValueObjectFactory.CreatePhoneNumber(request.PhoneNumber)
            );

            teacherToBeUpdated.UserId = request.UserId;

            await _teacherRepository.Update(teacherToBeUpdated, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return TeacherModelOutput.FromTeacher(teacherToBeUpdated);
        }
    }
}
