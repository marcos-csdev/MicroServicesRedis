using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.gRPCServices;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repositories;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController(IShoppingCartRepository cartRepository, Serilog.ILogger logger, IDiscountGrpcService discountGrpcService) : Controller
    {
        private readonly Serilog.ILogger _logger = logger;
        private readonly IShoppingCartRepository _cartRepository = cartRepository;
        private readonly IDiscountGrpcService _discountGrpcService;

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

        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                return BadRequest("No cart provided");
            try
            {
                //recalculates the current total price in the cart
                foreach (var item in shoppingCart.Items)
                {
                    var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);

                    //removing price from total
                    item.Price -= coupon.Amount;
                }

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
