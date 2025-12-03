using Microsoft.EntityFrameworkCore;
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.Repository;

namespace Treinou.Infraestructure.Repositories
{
    public class StudentRepository : IStudentRepository
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
            var student = await _students.FindAsync(id);
            if (student is null) throw new NotFoundException($"Student '{id}' not found");
            return student;

        }

        public async Task Insert(Student aggregate, CancellationToken cancellationToken)
            => await _students.AddAsync(aggregate);
        

        public async Task Update(Student aggregate, CancellationToken cancellationToken)
            => await Task.FromResult(_context.Update(aggregate));
        
    }
}
