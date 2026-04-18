using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Treinou.Domain.Enums;
using Treinou.Infraestructure.Identity;

namespace Treinou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(201, StatusCode = StatusCodes.Status201Created)]
        [ProducesResponseType(422, StatusCode = StatusCodes.Status422UnprocessableEntity, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Register([FromBody] RegisterInput input)
        {
            var user = new ApplicationUser
            {
                UserName = input.Email,
                Email = input.Email,
                UserType = input.UserType
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(new ProblemDetails
                {
                    Title = "Registration failed",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = string.Join("; ", result.Errors.Select(e => e.Description))
                });
            }

            return CreatedAtAction(nameof(Register), new RegisterOutput(user.Id, user.Email!, user.UserType));
        }
    }

    public class RegisterInput
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType UserType { get; set; }
    }

    public class RegisterOutput
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public RegisterOutput(string id, string email, UserType userType)
        {
            Id = id;
            Email = email;
            UserType = userType;
        }
    }
}
