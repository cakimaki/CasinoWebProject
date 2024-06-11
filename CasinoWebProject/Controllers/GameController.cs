using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CasinoWebProject.Models;
using CasinoWebProject.Services;
using CasinoWebProject.ViewModels;
using System.Threading.Tasks;
using CasinoWebProject.DTOs;

namespace CasinoWebProject.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGame _game;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamesController(IGame game, UserManager<ApplicationUser> userManager)
        {
            _game = game;
            _userManager = userManager;
        }

        [HttpPost("play")]
        public async Task<IActionResult> PlayGame([FromBody] BetViewModel model)
        {
            if (model.BetAmount <= 0)
            {
                return BadRequest(new { message = "Bet amount must be greater than zero." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            if (user.Balance < model.BetAmount)
            {
                return BadRequest(new { message = "Insufficient balance." });
            }

            var gameResult = _game.Play(model.BetAmount);

            if (gameResult.IsWin)
            {
                user.Balance += gameResult.Amount; // Add winnings
            }
            else
            {
                user.Balance -= model.BetAmount; // Subtract bet amount
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(new { message = "Failed to update user balance.", errors = updateResult.Errors });
            }

            return Ok(new { message = gameResult.Message, newBalance = user.Balance });
        }
    }
}
