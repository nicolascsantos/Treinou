using Treinou.Application.Adapters;
using Treinou.Application.UseCases.Teacher.Common;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork;

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
            // Use Adapter Pattern to convert Input DTO to Domain Entity
            var teacher = TeacherAdapter.ToEntity(request);

            await _teacherRepository.Insert(teacher, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            // Use Adapter Pattern to convert Domain Entity to Output DTO
            return TeacherAdapter.ToOutput(teacher);
        }
    }
}
