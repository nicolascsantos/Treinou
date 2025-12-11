using Treinou.Application.UseCases.Teacher.Common;
using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Teacher.GetTeacher
{
    public class GetTeacher : IGetTeacher
    {
        private readonly ITeacherRepository _teacherRepository;

        public GetTeacher(ITeacherRepository teacherRepository)
            => _teacherRepository = teacherRepository;  

        public async Task<TeacherModelOutput> Handle(GetTeacherInput request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.Get(request.Id, cancellationToken);

            return TeacherModelOutput.FromTeacher(teacher);
        }
    }
}
