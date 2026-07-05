using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using Treinou.Application.Services;
using Treinou.Infraestructure.Identity;

namespace Treinou.Application.UseCases.Auth
{
    public class LoginUser : ILoginUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginUser(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginUserOutput> Handle(LoginUserInput request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new AuthenticationException("Invalid credentials.");

            var (token, expiresAt) = _tokenService.GenerateToken(user.Id, user.Email!, user.UserType.ToString());
            return new LoginUserOutput(token, expiresAt);
        }
    }
}
