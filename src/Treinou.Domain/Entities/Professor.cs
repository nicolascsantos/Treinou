using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;
using Treinou.Domain.ValueObjects;

namespace Treinou.Domain.Entities
{
    public class Professor : Entity
    {
        public string Nome { get; set; }
        public CPF CPF { get; set; }
        public Email Email { get; set; }
        public Telefone Telefone { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public DateTime CriadoEm { get; set; }
        public bool Ativo { get; set; }

        public Professor(
            string nome,
            string cpf,
            string email,
            string telefone,
            DateTime dataDeNascimento,
            bool ativo = true
        )
        {
            Nome = nome;
            CPF = CPF.Criar(cpf);
            Email = Email.Criar(email);
            Telefone = Telefone.Criar(telefone);
            DataDeNascimento = dataDeNascimento;
            Ativo = ativo;
            CriadoEm = DateTime.Now;
            Validar();
        }

        public void Validar()
        {
            ValidacaoDominio.NaoNuloOuVazio(Nome, nameof(Nome));
        }

        public void Update(
            string nome,
            string cpf,
            string email,
            string telefone,
            DateTime dataDeNascimento
        )
        {
            Nome = nome;
            CPF = CPF.Criar(cpf);
            Email = Email.Criar(email);
            Telefone = Telefone;
            DataDeNascimento = dataDeNascimento;
            Validar();
        }
    }
}
