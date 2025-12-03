namespace Treinou.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? message) : base(message) {}

        public static void ThrowIfNull(object? @object, string exceptionMessage)
        {
            if (@object is null)
                throw new NotFoundException(exceptionMessage);
        }
    }
}
