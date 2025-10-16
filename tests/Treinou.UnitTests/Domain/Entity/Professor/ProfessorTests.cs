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
                professorValido.CPF.Numero,
                professorValido.Email.Endereco,
                professorValido.Telefone.Numero,
                professorValido.DataDeNascimento
            );

            var dataDepoisCriacao = DateTime.Now;

            professor.Should().NotBeNull();
            professor.Nome.Should().Be(professorValido.Nome);
            professor.Email.Endereco.Should().Be(professorValido.Email.Endereco);
            professor.CPF.Numero.Should().Be(professorValido.CPF.Numero);
            professor.Telefone.Numero.Should().Be(professorValido.Telefone.Numero);
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
                professorValido.CPF.Numero,
                professorValido.Email.Endereco,
                professorValido.Telefone.Numero,
                professorValido.DataDeNascimento,
                ativo
            );

            var dataDepoisCriacao = DateTime.Now;

            professor.Should().NotBeNull();
            professor.Nome.Should().Be(professorValido.Nome);
            professor.Email.Endereco.Should().Be(professorValido.Email.Endereco);
            professor.CPF.Numero.Should().Be(professorValido.CPF.Numero);
            professor.Telefone.Numero.Should().Be(professorValido.Telefone.Numero);
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
                professorValido.CPF.Numero,
                professorValido.Email.Endereco,
                professorValido.Telefone.Numero,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("Nome não pode ser nulo ou vazio.");
        }

        [Theory(DisplayName = nameof(ExcecaoQuandoCPFForVazio))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void ExcecaoQuandoCPFForVazio(string? cpfNuloOuVazio)
        {
            var professorValido = _fixture.GetProfessorValido();
            Action action = () => new Entidades.Professor(
                professorValido.Nome,
                cpfNuloOuVazio!,
                professorValido.Email.Endereco,
                professorValido.Telefone.Numero,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("CPF não pode ser nulo ou vazio.");
        }

        [Theory(DisplayName = nameof(ExcecaoQuandoCPFForInvalido))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData("12312312312")]
        [InlineData("121212")]
        public void ExcecaoQuandoCPFForInvalido(string? cpfNuloOuVazio)
        {
            var professorValido = _fixture.GetProfessorValido();
            Action action = () => new Entidades.Professor(
                professorValido.Nome,
                cpfNuloOuVazio!,
                professorValido.Email.Endereco,
                professorValido.Telefone.Numero,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("CPF inválido.");
        }

        [Theory(DisplayName = nameof(ExcecaoQuandoEmailForVazio))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void ExcecaoQuandoEmailForVazio(string? emailInvalido)
        {
            var professorValido = _fixture.GetProfessorValido();
            Action action = () => new Entidades.Professor(
                professorValido.Nome,
                professorValido.CPF.Numero,
                emailInvalido!,
                professorValido.Telefone.Numero,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("E-mail não pode ser vazio ou nulo.");
        }

        [Theory(DisplayName = nameof(ExcecaoQuandoTelefoneForVazioOuNulo))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void ExcecaoQuandoTelefoneForVazioOuNulo(string? telefoneVazioOuNulo)
        {
            var professorValido = _fixture.GetProfessorValido();
            Action action = () => new Entidades.Professor(
                professorValido.Nome,
                professorValido.CPF.Numero,
                professorValido.Email.Endereco,
                telefoneVazioOuNulo!,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("Telefone não pode ser vazio ou nulo.");
        }

        [Theory(DisplayName = nameof(ExcecaoQuandoTelefoneForInvalido))]
        [Trait("Domain", "Professor - Entities")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("13333")]
        [InlineData("1222222222")]
        public void ExcecaoQuandoTelefoneForInvalido(string? telefoneInvalido)
        {
            var professorValido = _fixture.GetProfessorValido();
            Action action = () => new Entidades.Professor(
                professorValido.Nome,
                professorValido.CPF.Numero,
                professorValido.Email.Endereco,
                telefoneInvalido!,
                professorValido.DataDeNascimento
            );
            action.Should().Throw<EntityValidationException>()
                .WithMessage("Telefone não pode ser vazio ou nulo.");
        }
    }
}
