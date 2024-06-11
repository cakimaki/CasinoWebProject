using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CasinoWebProject.Models;
using CasinoWebProject.ViewModels;

namespace CasinoWebProject.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("add-money")]
        public async Task<IActionResult> AddMoney([FromBody] TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found" });
                }

                // Simulate transaction processing (card validation, payment gateway integration)
                bool transactionSuccessful = ProcessPayment(model);

                if (!transactionSuccessful)
                {
                    return BadRequest(new { message = "Transaction failed" });
                }

                user.Balance += model.Amount;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Money added successfully", newBalance = user.Balance });
                }
                else
                {
                    return BadRequest(new { message = "Failed to update balance", errors = result.Errors });
                }
            }
            return BadRequest(ModelState);
        }

        private bool ProcessPayment(TransactionViewModel model)
        {
            // todo
            // integrate a payment gateway (paypal, stripe etc)
            return true;
        }
    }
}
