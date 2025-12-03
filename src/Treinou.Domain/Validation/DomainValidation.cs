using Treinou.Domain.Exceptions;

namespace Treinou.Domain.Validation
{
    public static class DomainValidation
    {
        public static void NotNull(object? target, string fieldName)
        {
            if (target is null)
                throw new EntityValidationException($"{fieldName} should not be null.");
        }

        public static void NotNullOrEmpty(string? target, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(target))
                throw new EntityValidationException($"{fieldName} should not be empty or null.");
        }

        public static void MinLength(string target,string fieldName, int minLength)
        {
            if (target.Length < minLength)
                throw new EntityValidationException($"{fieldName} should be at least {minLength} characters long.");
        }

        public static void MaxLength(string target, string fieldName, int maxLength)
        {
            if (target.Length > maxLength)
                throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characters long.");
        }
    }
}
