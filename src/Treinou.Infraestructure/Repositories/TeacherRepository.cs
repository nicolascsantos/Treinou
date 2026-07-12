using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
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
            var teacher = await _teachers
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (teacher is null) throw new NotFoundException($"Teacher '{id}' not found");
            return teacher;
        }

        public async Task Insert(Teacher aggregate, CancellationToken cancellationToken)
            => await _teachers.AddAsync(aggregate);

        public async Task Update(Teacher aggregate, CancellationToken cancellationToken)
            => _teachers.Update(aggregate);

        public IQueryable<Teacher> AddOrderToQuery(IQueryable<Teacher> query, string propertyToOrderBy, SearchOrder order)
            => (propertyToOrderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.ASCENDING) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Name),
                ("email", SearchOrder.ASCENDING) => query.OrderBy(x => x.Email.Address),
                ("email", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Email.Address),
                ("cpf", SearchOrder.ASCENDING) => query.OrderBy(x => x.CPF.Number),
                ("cpf", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.CPF.Number),
                ("cref", SearchOrder.ASCENDING) => query.OrderBy(x => x.CREF.Number),
                ("cref", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.CREF.Number),
                ("phonenumber", SearchOrder.ASCENDING) => query.OrderBy(x => x.PhoneNumber.Number),
                ("phonenumber", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.PhoneNumber.Number),
                ("birthdate", SearchOrder.ASCENDING) => query.OrderBy(x => x.BirthDate),
                ("birthdate", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.BirthDate),
                _ => query.OrderBy(x => x.Name)
            };

        public async Task<SearchOutput<Teacher>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _teachers.AsNoTracking().AsQueryable();
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Name.Contains(searchInput.Search));
            var total = await query.CountAsync();
            var teachers = await query.Skip(toSkip)
                .Take(searchInput.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<Teacher>(
                searchInput.Page,
                searchInput.PerPage,
                total,
                teachers
            );
        }
    }
}
