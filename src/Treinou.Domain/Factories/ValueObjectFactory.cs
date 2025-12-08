using Treinou.Domain.ValueObjects;

namespace Treinou.Domain.Factories
{
    /// <summary>
    /// Factory Method Pattern implementation for creating ValueObjects.
    /// Centralizes creation logic, validation, and normalization of ValueObjects.
    /// </summary>
    public static class ValueObjectFactory
    {
        /// <summary>
        /// Creates an Email ValueObject with validation and normalization.
        /// </summary>
        /// <param name="address">The email address</param>
        /// <returns>A normalized Email ValueObject</returns>
        public static Email CreateEmail(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email address cannot be null or empty", nameof(address));

            // Normalize: trim and convert to lowercase
            var normalizedEmail = address.Trim().ToLowerInvariant();

            return new Email(normalizedEmail);
        }

        /// <summary>
        /// Creates a CPF ValueObject with validation and formatting.
        /// </summary>
        /// <param name="number">The CPF number (can contain dots and hyphens)</param>
        /// <returns>A formatted CPF ValueObject</returns>
        public static CPF CreateCPF(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("CPF number cannot be null or empty", nameof(number));

            // Remove special characters (dots and hyphens)
            var cleanedCPF = number.Replace(".", "").Replace("-", "").Trim();

            // Basic validation: CPF must have 11 digits
            if (cleanedCPF.Length != 11 || !cleanedCPF.All(char.IsDigit))
                throw new ArgumentException("CPF must contain exactly 11 digits", nameof(number));

            return new CPF(cleanedCPF);
        }

        /// <summary>
        /// Creates a PhoneNumber ValueObject with validation and formatting.
        /// </summary>
        /// <param name="number">The phone number (can contain special characters)</param>
        /// <returns>A formatted PhoneNumber ValueObject</returns>
        public static PhoneNumber CreatePhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Phone number cannot be null or empty", nameof(number));

            // Remove common special characters
            var cleanedPhone = number
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();

            // Basic validation: must contain only digits and have reasonable length
            if (!cleanedPhone.All(char.IsDigit) || cleanedPhone.Length < 10 || cleanedPhone.Length > 11)
                throw new ArgumentException("Phone number must contain 10 or 11 digits", nameof(number));

            return new PhoneNumber(cleanedPhone);
        }

        /// <summary>
        /// Creates a CREF ValueObject with validation and formatting.
        /// </summary>
        /// <param name="number">The CREF number</param>
        /// <returns>A formatted CREF ValueObject</returns>
        public static CREF CreateCREF(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("CREF number cannot be null or empty", nameof(number));

            // Normalize: trim and convert to uppercase (CREF usually uses uppercase)
            var normalizedCREF = number.Trim().ToUpperInvariant();

            return new CREF(normalizedCREF);
        }
    }
}
