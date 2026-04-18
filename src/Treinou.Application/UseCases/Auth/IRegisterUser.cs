using MediatR;

namespace Treinou.Application.UseCases.Auth
{
    public interface IRegisterUser : IRequestHandler<RegisterUserInput, RegisterUserOutput>
    {
    }
}
