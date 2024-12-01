using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using united_movers_api.Models;
using united_movers_api.Services.Interfaces;

namespace united_movers_api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: auth/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            // Error checks

            if (String.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { message = "User name needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            // Try login

            var loggedInUser = await _authService.Login(user);

            // Return responses

            if (loggedInUser != null)
            {
                return Ok(loggedInUser);
            }

            return BadRequest(new { message = "User login unsuccessful" });
        }

        // POST: auth/register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser newuser)
        {
            // Error checks

            if (String.IsNullOrEmpty(newuser.Name))
            {
                return BadRequest(new { message = "Name needs to entered" });
            }
            else if (String.IsNullOrEmpty(newuser.UserName))
            {
                return BadRequest(new { message = "User name needs to entered" });
            }
            else if (String.IsNullOrEmpty(newuser.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            // Try registration

            User user = new User();
            user.Username = newuser.UserName;
            user.Name = newuser.Name;
            user.Email = newuser.Email;
            user.PasswordHash = newuser.Password;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            var registeredUser = await _authService.Register(user);

            // Return responses

            if (registeredUser != null)
            {
                return Ok(registeredUser);
            }

            return BadRequest(new { message = "User registration unsuccessful" });
        }

    }
}
