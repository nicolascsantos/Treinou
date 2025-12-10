using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class TeacherRepository : ITeacherRepository, ISearchableRepository<Teacher>
    {
        private readonly TreinouDbContext _context;

        private DbSet<Teacher> _teachers => _context.Set<Teacher>();

        public TeacherRepository(TreinouDbContext context)
            => _context = context;

        public Task Delete(Teacher aggregate, CancellationToken cancellationToken)
        {
            _teachers.Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<Teacher> Get(Guid id, CancellationToken cancellationToken)
        {
            var teacher = await _teachers.FindAsync(id);
            if (teacher is null) throw new NotFoundException($"Teacher '{id}' not found");
            return teacher;
        }

        public async Task Insert(Teacher aggregate, CancellationToken cancellationToken)
            => await _teachers.AddAsync(aggregate);

        public async Task Update(Teacher aggregate, CancellationToken cancellationToken)
            => _teachers.Update(aggregate);

        public IQueryable<Teacher> AddOrderToQuery(IQueryable<Teacher> query, string propertyToOrderBy, SearchOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<SearchOutput<Teacher>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
