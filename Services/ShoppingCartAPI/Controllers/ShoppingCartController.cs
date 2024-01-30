using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartAPI.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
