using Bogus;
using System;

namespace Treinou.UnitTests.Common
{
    public class BaseFixture
    {
        public Faker Faker { get; set; }

        private readonly Random _random = new Random();

        public BaseFixture()
            => Faker = new Faker("pt_BR");
        

        public bool BooleanoRandomico() => new Random().NextDouble() < 0.5;

        public string GetCPFValido()
        {
            int[] cpf = new int[11];

            for (int i = 0; i < 9; i++)
                cpf[i] = _random.Next(0, 10);

            cpf[9] = CalcularDigito(cpf, 9);
            cpf[10] = CalcularDigito(cpf, 10);

            string resultado = string.Concat(cpf);

            return resultado;
        }

        private static int CalcularDigito(int[] cpf, int pos)
        {
            int soma = 0;
            for (int i = 0; i < pos; i++)
                soma += cpf[i] * (pos + 1 - i);

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}
