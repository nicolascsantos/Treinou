using Treinou.Application.UseCases.Student.Common;
using Treinou.Domain.Factories;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Student.UpdateStudent
{
    public class UpdateStudent : IUpdateStudent
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudent(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
            => (_studentRepository, _unitOfWork) = (studentRepository, unitOfWork);


        public async Task<StudentModelOutput> Handle(UpdateStudentInput request, CancellationToken cancellationToken)
        {
            var studentToBeUpdated = await _studentRepository.Get(request.Id, cancellationToken);

            studentToBeUpdated.Update(
                request.Name,
                ValueObjectFactory.CreateEmail(request.Email),
                ValueObjectFactory.CreateCPF(request.CPF),
                ValueObjectFactory.CreatePhoneNumber(request.PhoneNumber),
                request.Weight,
                request.Height
            );

            await _studentRepository.Update(studentToBeUpdated, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return StudentModelOutput.FromStudent(studentToBeUpdated);
        }
    }
}
