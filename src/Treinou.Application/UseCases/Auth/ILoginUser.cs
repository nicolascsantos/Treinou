using MediatR;

namespace Treinou.Application.UseCases.Auth
{
    public interface ILoginUser : IRequestHandler<LoginUserInput, LoginUserOutput>
    {
    }
}
