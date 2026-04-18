using Microsoft.AspNetCore.Identity;
using Treinou.Domain.Enums;

namespace Treinou.Infraestructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserType UserType { get; set; }
    }
}
