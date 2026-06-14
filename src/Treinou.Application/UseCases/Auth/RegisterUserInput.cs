using MediatR;

namespace Treinou.Application.UseCases.Auth
{
    public class RegisterUserInput : IRequest<RegisterUserOutput>
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public RegisterUserInput(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
