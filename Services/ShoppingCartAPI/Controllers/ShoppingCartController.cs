using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repositories;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : Controller
    {
        private readonly Serilog.ILogger _logger;
        private readonly IShoppingCartRepository _cartRepository;

        public ShoppingCartController(IShoppingCartRepository cartRepository, Serilog.ILogger logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        [HttpGet("{userName:maxlength(30)}", Name = "Get")]
        public async Task<IActionResult> Get(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("No userName provided");
            try
            {
                var cart = await _cartRepository.GetCartAsync(userName);

                if (cart != null) return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);

                return Problem($"{ex.Message} ");
            }

            return Problem("Could not retrieve cart");
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                return BadRequest("No cart provided");
            try
            {
                var result = await _cartRepository.UpsertCartAsync(shoppingCart);

                if (result != null) return Ok("Update successful");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not update cart");
        }

        [HttpDelete("{userName:maxlength(30)}", Name = "Delete")]
        public async Task<IActionResult> Delete(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("No userName provided");
            try
            {
                await _cartRepository.DeleteCartAsync(userName);

                return Ok("Delete successful");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not delete cart");
        }

    }
}
