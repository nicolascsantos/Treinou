using Treinou.Application.Adapters;
using Treinou.Application.UseCases.Student.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

namespace Treinou.Application.UseCases.Student.CreateStudent
{
    public class CreateStudent : ICreateStudent
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudent(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            IUnitOfWork unitOfWork
        )
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentModelOutput> Handle(
            CreateStudentInput request,
            CancellationToken cancellationToken
        )
        {
            // Use Adapter Pattern to convert Input DTO to Domain Entity
            var student = StudentAdapter.ToEntity(request);

            if (student.TeacherId != default)
            {
                var teacher = await _teacherRepository.Get(student.TeacherId, cancellationToken);
                student.Teacher = teacher;
            }

            await _studentRepository.Insert(student, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            // Use Adapter Pattern to convert Domain Entity to Output DTO
            return StudentAdapter.ToOutput(student);
        }
    }
}
