using Treinou.Application.UseCases.Teacher.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;
using Treinou.Domain.ValueObjects;

namespace Treinou.Application.UseCases.Teacher.CreateTeacher
{
    public class CreateTeacher : ICreateTeacher
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeacher(
            ITeacherRepository teacherRepository,
            IUnitOfWork unitOfWork
        )
        {
            _teacherRepository = teacherRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TeacherModelOutput> Handle(
            CreateTeacherInput request,
            CancellationToken cancellationToken
        )
        {
            var email = new Email(request.Email);
            var cpf = new CPF(request.CPF);
            var cref = new CREF(request.CREF);
            var phoneNumber = new PhoneNumber(request.PhoneNumber);

            var teacher = new Domain.Entities.Teacher(
                request.Name,
                email,
                cpf,
                cref,
                request.Description,
                phoneNumber,
                request.BirthDate,
                DateTime.Now
            );

            await _teacherRepository.Insert(teacher, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return TeacherModelOutput.FromTeacher(teacher);
        }
    }
}
