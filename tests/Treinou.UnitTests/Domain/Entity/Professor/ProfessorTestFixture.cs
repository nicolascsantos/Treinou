using System;
using Treinou.UnitTests.Common;
using Entidades = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entity.Professor
{
    [CollectionDefinition(nameof(ProfessorTestFixture))]
    public class ProfessorTestFixtureCollection : ICollectionFixture<ProfessorTestFixture> { }

    public class ProfessorTestFixture : BaseFixture
    {
        public Entidades.Professor GetProfessorValido()
            => new Entidades.Professor(
                GetNomeProfessorValido(),
                GetCPFValido(),
                GetEmailValido(),
                GetTelefoneValido(),
                GetDataNascimentoValida(),
                BooleanoRandomico()
            );

        public string GetNomeProfessorValido()
            => Faker.Name.FullName();

        public string GetEmailValido()
            => Faker.Internet.Email();
        
        public string GetTelefoneValido()
            => Faker.Phone.PhoneNumber("(##) #####-####");
        
        public DateTime GetDataNascimentoValida()
            => Faker.Person.DateOfBirth.Date;
    }
}
