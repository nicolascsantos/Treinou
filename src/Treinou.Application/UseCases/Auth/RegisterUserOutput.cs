using Treinou.Domain.Enums;

namespace Treinou.Application.UseCases.Auth
{
    public class RegisterUserOutput
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public RegisterUserOutput(string id, string email, UserType userType)
        {
            Id = id;
            Email = email;
            UserType = userType;
        }
    }
}