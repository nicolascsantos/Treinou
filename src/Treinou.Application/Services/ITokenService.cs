namespace Treinou.Application.Services
{
    public interface ITokenService
    {
        (string token, DateTime expiresAt) GenerateToken(string userId, string email, string userType);
    }
}
