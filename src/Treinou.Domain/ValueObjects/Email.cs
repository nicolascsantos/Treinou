using System.Text.RegularExpressions;
using Treinou.Domain.Exceptions;

namespace Treinou.Domain.ValueObjects
{
    public class Email : IEquatable<Email>
    {
        public string Endereco { get; set; }

        public Email(string email)
            => Endereco = Validar(email);
        

        public bool Equals(Email? other)
            => Equals(other as Email);

        public static Email Criar(string email)
            => new Email(email);
        

        public static string Validar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new EntityValidationException("E-mail não pode ser vazio ou nulo.");

            email = email.Trim().ToLowerInvariant();

            if (!email.Contains("@"))
                throw new EntityValidationException("E-mail deve conter '@'");

            string[] partesEmail = email.Split('@');

            if (partesEmail.Length != 2 || string.IsNullOrWhiteSpace(partesEmail[0]) || string.IsNullOrWhiteSpace(partesEmail[1]))
            {
                throw new EntityValidationException("E-mail inválido");
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new EntityValidationException("Formato de e-mail inválido.");

            return email;
        }
    }
}
