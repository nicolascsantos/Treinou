using Treinou.Application.UseCases.Student.Common;
using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Student.GetStudent
{
    public class GetStudent : IGetStudent
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudent(IStudentRepository studentRepository)
        => _studentRepository = studentRepository;

        public async Task<StudentModelOutput> Handle(GetStudentInput request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.Get(request.Id, cancellationToken);

            return StudentModelOutput.FromStudent(student);
        }
    }
}
