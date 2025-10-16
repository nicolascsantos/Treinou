using Treinou.Domain.Exceptions;

namespace Treinou.Domain.ValueObjects
{
    public class CPF : IEquatable<CPF>
    {
        public string Numero { get; private set; }

        public CPF(string numero)
            => Numero = numero;
        

        public static CPF Criar(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new EntityValidationException("CPF não pode ser nulo ou vazio.");

            var cpfLimpo = LimparCPF(cpf);

            if (!Validar(cpfLimpo))
                throw new EntityValidationException("CPF inválido.");

            return new CPF(cpfLimpo);
        }

        private static string LimparCPF(string cpf)
        {
            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        private static bool Validar(string cpf)
        {
            // CPF deve ter 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula o primeiro dígito verificador
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;

            // Calcula o segundo dígito verificador
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digito2;
        }

        public bool Equals(CPF? other)
        {
            throw new NotImplementedException();
        }
    }
}
