using Treinou.Domain.Exceptions;

namespace Treinou.Domain.Validation
{
    public static class ValidacaoDominio
    {
        public static void NaoNulo(object? alvo, string nome)
        {
            if (alvo == null) 
                throw new EntityValidationException($"{nome} não pode ser nulo.");
        }

        public static void NaoNuloOuVazio(string? alvo, string nome)
        {
            if (string.IsNullOrWhiteSpace(alvo))
                throw new EntityValidationException($"{nome} não pode ser nulo ou vazio.");
        }

        public static void TamanhoMinimo(string alvo, int tamanhoMinimo, string nomeCampo)
        {
            if (alvo.Length < tamanhoMinimo)
                throw new EntityValidationException($"{nomeCampo} deve ter no mínimo {tamanhoMinimo} caracteres");
        }

        public static void TamanhoMaximo(string alvo, int tamanhoMaximo, string nomeCampo)
        {
            if (alvo.Length > tamanhoMaximo)
                throw new EntityValidationException($"{nomeCampo} deve ter no mínimo {tamanhoMaximo} caracteres");
        }
    }
}
