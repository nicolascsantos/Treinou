using System.Text.RegularExpressions;
using Treinou.Domain.Exceptions;

namespace Treinou.Domain.ValueObjects
{
    public class Telefone : IEquatable<Telefone>
    {
        public string Numero { get; set; }

        public string CodigoPais { get; set; }

        public string CodigoArea { get; set; }


        public Telefone(string numero, string codigoPais = "55")
        {
            Numero = numero;
            CodigoPais = codigoPais;
            CodigoArea = ExtrairDDD(numero);
        }

        public static Telefone Criar(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new EntityValidationException("Telefone não pode ser vazio ou nulo.");

            numero = LimparNumero(numero);

            if (numero.Length < 10 || numero.Length > 11)
                throw new EntityValidationException("Número de telefone inválido.");

            return new Telefone(numero);
        }

        public string ExtrairDDD(string numero)
            => numero.Length >= 2 ? numero.Substring(0, 2) : "";
        

        public string FormatarBrasileiro()
        {
            if (Numero.Length == 11) // Celular
                return $"({CodigoArea}) {Numero.Substring(2, 5)}-{Numero.Substring(6, 10)}";
            

            if (Numero.Length == 10) // Fixo
                return $"({CodigoArea}) {Numero.Substring(2, 5)}-{Numero.Substring(6, 10)}";
            return Numero;
        }

        private static string LimparNumero(string numero)
            => Regex.Replace(numero, @"[^\d]", "");


        public bool Equals(Telefone? other)
            => Equals(other as Telefone);
    }
}
