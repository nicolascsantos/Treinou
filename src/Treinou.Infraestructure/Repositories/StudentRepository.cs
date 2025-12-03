using Treinou.Domain.Entities;
using Treinou.Domain.Repository;

namespace Treinou.Infraestructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        public Task Delete(Student aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Get(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Insert(Student aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(Student aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
