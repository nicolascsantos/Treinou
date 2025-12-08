using Treinou.Application.UseCases.Student.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;
using Treinou.Domain.ValueObjects;

namespace Treinou.Application.UseCases.Student.CreateStudent
{
    public class CreateStudent : ICreateStudent
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudent(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork
        )
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentModelOutput> Handle(
            CreateStudentInput request,
            CancellationToken cancellationToken
        )
        {
            var email = new Email(request.Email);
            var cpf = new CPF(request.CPF);
            var phoneNumber = new PhoneNumber(request.PhoneNumber);

            var student = new Domain.Entities.Student(
                request.Name,
                email,
                cpf,
                phoneNumber,
                request.Weight,
                request.Height,
                request.IsActive
            );

            student.TeacherId = request.TeacherId;

            await _studentRepository.Insert(student, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return StudentModelOutput.FromStudent(student);
        }
    }
}
