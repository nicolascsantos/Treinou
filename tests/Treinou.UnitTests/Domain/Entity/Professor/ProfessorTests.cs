using FluentAssertions;
using Treinou.Domain.Exceptions;
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

        [Theory(DisplayName = nameof(InstanciarComStatus))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstanciarComStatus(bool ativo)
        {
            var professorValido = _fixture.GetProfessorValido();

            var dataAntesCriacao = DateTime.Now;

            var professor = new Entidades.Professor
            (
                professorValido.Nome,
                professorValido.CPF,
                professorValido.Email,
                professorValido.Telefone,
                professorValido.DataDeNascimento,
                ativo
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
            professor.Ativo.Should().Be(ativo);
        }

        [Theory(DisplayName = nameof(ExcecaoQuandoNomeForVazio))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void ExcecaoQuandoNomeForVazio(string? nome)
        {
            var professorValido = _fixture.GetProfessorValido();
            Action action = () => new Entidades.Professor(
                nome!,
                professorValido.CPF,
                professorValido.Email,
                professorValido.Telefone,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("Nome não pode ser nulo ou vazio.");
        }
    }
}
