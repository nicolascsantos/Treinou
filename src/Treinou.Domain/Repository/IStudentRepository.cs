using Treinou.Domain.Entities;
using Treinou.Domain.SeedWork;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Domain.Repository
{
    public interface IStudentRepository : ISearchableRepository<Student>, IGenericRepository<Student>
    {

    }
}
