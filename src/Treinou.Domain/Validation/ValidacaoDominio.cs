using Treinou.Domain.Exceptions;

namespace Treinou.Domain.Validation
{
    public static class ValidacaoDominio
    {
        public static void NaoNuloOuVazio(string? alvo, string nome)
        {
            if (string.IsNullOrWhiteSpace(alvo))
                throw new EntityValidationException("Nome não pode ser nulo ou vazio.");
        }
    }
}
