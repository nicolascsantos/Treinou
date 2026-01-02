using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Infraestructure.Repositories
{
    public class StudentRepository : IStudentRepository, ISearchableRepository<Student>
    {
        private readonly TreinouDbContext _context;

        private DbSet<Student> _students => _context.Set<Student>();

        public StudentRepository(TreinouDbContext context)
            => _context = context;

        public Task Delete(Student aggregate, CancellationToken cancellationToken)
        {
            _students.Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<Student> Get(Guid id, CancellationToken cancellationToken)
        {
            var student = await _students.Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            if (student is null) throw new NotFoundException($"Student '{id}' not found");
            return student;

        }

        public async Task Insert(Student aggregate, CancellationToken cancellationToken)
            => await _students.AddAsync(aggregate);


        public async Task Update(Student aggregate, CancellationToken cancellationToken)
            => await Task.FromResult(_context.Update(aggregate));

        public async Task<SearchOutput<Student>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _students.AsNoTracking();
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Name.Contains(searchInput.Search));
            var total = await query.CountAsync();
            var students = await query.Skip(toSkip)
                .Take(searchInput.PerPage)
                .ToListAsync(cancellationToken);

            return new SearchOutput<Student>(
                searchInput.Page,
                searchInput.PerPage,
                total,
                students
            );
        }

        public IQueryable<Student> AddOrderToQuery(IQueryable<Student> query, string propertyToOrderBy, SearchOrder order)
            => (propertyToOrderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.ASCENDING) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Name),
                ("email", SearchOrder.ASCENDING) => query.OrderBy(x => x.Email.Address),
                ("email", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Email.Address),
                ("cpf", SearchOrder.ASCENDING) => query.OrderBy(x => x.CPF.Number),
                ("cpf", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.CPF.Number),
                ("phonenumber", SearchOrder.ASCENDING) => query.OrderBy(x => x.PhoneNumber.Number),
                ("phonenumber", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.PhoneNumber.Number),
                ("birthdate", SearchOrder.ASCENDING) => query.OrderBy(x => x.BirthDate),
                ("birthdate", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.BirthDate),
                ("weight", SearchOrder.ASCENDING) => query.OrderBy(x => x.Weight),
                ("weight", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Weight),
                ("height", SearchOrder.ASCENDING) => query.OrderBy(x => x.Height),
                ("height", SearchOrder.DESCENDING) => query.OrderByDescending(x => x.Height),
                _ => query.OrderBy(x => x.Name)
            };
    }
}
