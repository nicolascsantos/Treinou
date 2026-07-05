using MediatR;

namespace Treinou.Application.UseCases.Auth
{
    public class LoginUserInput : IRequest<LoginUserOutput>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public LoginUserInput(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
