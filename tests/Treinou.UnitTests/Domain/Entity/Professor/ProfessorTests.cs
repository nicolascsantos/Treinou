using FluentAssertions;
using Entidades = Treinou.Domain.Entities;

namespace Treinou.UnitTests.Domain.Entity.Professor
{
    [Collection(nameof(ProfessorTestFixture))]
    public class ProfessorTests
    {
        private readonly ProfessorTestFixture _fixture;

        public ProfessorTests(ProfessorTestFixture fixture)
            => _fixture = fixture;


        [Fact(DisplayName = nameof(Instanciar))]
        [Trait("Domain", "Professor - Entities")]
        public void Instanciar()
        {
            var professorValido = _fixture.GetProfessorValido();

            var dataAntesCriacao = DateTime.Now;

            var professor = new Entidades.Professor
            (
                professorValido.Nome,
                professorValido.CPF,
                professorValido.Email,
                professorValido.Telefone,
                professorValido.DataDeNascimento
            );

            var dataDepoisCriacao = DateTime.Now;

            professor.Should().NotBeNull();
            professor.Nome.Should().Be(professorValido.Nome);
            professor.Email.Should().Be(professorValido.Email);
            professor.CPF.Should().Be(professorValido.CPF);
            professor.Telefone.Should().Be(professorValido.Telefone);
            professor.DataDeNascimento.Should().Be(professorValido.DataDeNascimento);
            professor.CriadoEm.Should().NotBeSameDateAs(default);
            (professor.CriadoEm >= dataAntesCriacao).Should().BeTrue();
            (professor.CriadoEm <= dataDepoisCriacao).Should().BeTrue();
            professor.Ativo.Should().BeTrue();
        }
    }
}
