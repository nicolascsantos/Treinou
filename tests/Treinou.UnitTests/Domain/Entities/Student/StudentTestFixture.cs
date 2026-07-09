using Treinou.Domain.ValueObjects;
using Treinou.UnitTests.Common;
using DomainEntities = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entities.Student
{
    [CollectionDefinition(nameof(StudentTestFixture))]
    public class StudentTestFixtureCollection : ICollectionFixture<StudentTestFixture> { }
    public class StudentTestFixture : BaseFixture
    {
        public DomainEntities.Student GetValidStudent()
            => new(
                GetValidName(),
                GetValidEmail(),
                GetValidCPF(),
                GetValidPhoneNumber(),
                GetValidWeight(),
                GetValidHeight(),
                GetRandomBoolean()
            );

        public string GetValidName() => Faker.Name.FullName();

        public Email GetValidEmail() => new(Faker.Person.Email);

        public CPF GetValidCPF() => new("10223229075");

        public PhoneNumber GetValidPhoneNumber() => new("13999999999");

        public double GetValidWeight()
        {
            Random random = new();
            double minimumWight = 45.0;
            double maximumWeight = 120.0;

            double finalWeight = minimumWight + (random.NextDouble() * (maximumWeight - minimumWight));

            return Math.Round(finalWeight, 2);
        }

        public static double GetValidHeight()
        {
            Random random = new();
            double minimumHeight = 1.50;
            double maximumHeight = 2.10;

            double finalHeight = minimumHeight + (random.NextDouble() * (maximumHeight - minimumHeight));

            return Math.Round(finalHeight, 2);
        }

        public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    }
}
