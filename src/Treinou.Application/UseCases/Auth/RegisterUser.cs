using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using Treinou.Domain.Enums;
using Treinou.Infraestructure.Identity;

namespace Treinou.Application.UseCases.Auth
{
    public class RegisterUser : IRegisterUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUser(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;
        

        public async Task<RegisterUserOutput> Handle(RegisterUserInput request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                UserType = UserType.Student   // hardcoded default; teacher elevation requires a separate admin flow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) 
                throw new AuthenticationException(
                    "Registration failed.",
                    new Exception(string.Join("; ", result.Errors.Select(e => e.Description)))
                ); 

            return new RegisterUserOutput(user.Id, user.Email, user.UserType);
        }
    }
}
