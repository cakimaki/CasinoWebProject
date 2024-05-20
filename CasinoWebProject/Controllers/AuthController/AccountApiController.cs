using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CasinoWebProject.Models;
using System.Threading.Tasks;

namespace CasinoWebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Add this attribute to enforce API conventions
    public class AccountApiController : ControllerBase // Use ControllerBase for API controllers
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountApiController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { message = "Registration successful!" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Login successful!" });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid login attempt." });
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout successful!" });
        }

        [HttpGet("isloggedin")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new { isLoggedIn = User.Identity.IsAuthenticated });
        }
    }
}
