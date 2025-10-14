using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class Professor : Entity
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public DateTime CriadoEm { get; set; }
        public bool Ativo { get; set; }

        public Professor(
            string nome,
            string cpf,
            string email,
            string telefone,
            DateTime dataNascimento,
            bool ativo = true
        )
        {
            Nome = nome;
            CPF = cpf;
            Email = email;
            Telefone = telefone;
            DataDeNascimento = dataNascimento;
            Ativo = ativo;
            CriadoEm = DateTime.Now;
            Validar();
        }

        public void Validar()
        {
            ValidacaoDominio.NaoNuloOuVazio(Nome, nameof(Nome));
        }
    }
}
