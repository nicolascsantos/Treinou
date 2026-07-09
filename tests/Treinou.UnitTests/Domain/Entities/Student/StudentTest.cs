using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.Student
{
    [Collection(nameof(StudentTestFixture))]
    public class StudentTest
    {
        private readonly StudentTestFixture _fixture;

        public StudentTest(StudentTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Student - Aggregates")]
        public void Instantiate()
        {
            var validStudent = _fixture.GetValidStudent();

            var dateTimeBefore = DateTime.Now;

            var newStudent = new DomainEntities.Student
            (
                validStudent.Name,
                validStudent.Email,
                validStudent.CPF,
                validStudent.PhoneNumber,
                validStudent.Weight,
                validStudent.Height,
                validStudent.IsActive
            );

            var dateTimeAfter = DateTime.Now;

            Assert.Equal(newStudent.Name, validStudent.Name);
            Assert.Equal(newStudent.Email, validStudent.Email);
            Assert.Equal(newStudent.CPF, validStudent.CPF);
            Assert.Equal(newStudent.PhoneNumber, validStudent.PhoneNumber);
            Assert.Equal(newStudent.Weight, validStudent.Weight);
            Assert.Equal(newStudent.Weight, validStudent.Weight);
            Assert.Equal(newStudent.IsActive, validStudent.IsActive);
        }
    }
}
