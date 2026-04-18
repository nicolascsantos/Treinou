using MediatR;
using System.Text.Json.Serialization;
using Treinou.Domain.Enums;

namespace Treinou.Application.UseCases.Auth
{
    public class RegisterUserInput : IRequest<RegisterUserOutput>
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType UserType { get; set; }

        public RegisterUserInput(string email, string password, UserType userType)
        {
            Email = email;
            Password = password;
            UserType = userType;
        }
    }
}