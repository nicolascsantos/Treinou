using MediatR;
using Treinou.Application.UseCases.Teacher.Common;

namespace Treinou.Application.UseCases.Teacher.GetTeacher
{
    public class GetTeacherInput : IRequest<TeacherModelOutput>
    {
        public Guid Id { get; private set; }

        public GetTeacherInput(Guid id) 
            => Id = id;
    }
}
