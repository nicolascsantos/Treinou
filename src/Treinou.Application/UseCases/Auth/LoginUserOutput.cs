namespace Treinou.Application.UseCases.Auth
{
    public class LoginUserOutput
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public LoginUserOutput(string token, DateTime expiresAt)
        {
            Token = token;
            ExpiresAt = expiresAt;
        }
    }
}
