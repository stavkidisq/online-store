using Microsoft.AspNetCore.Mvc;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public CartController(OnlineStoreDbContext context)
        {
            _context = context;
        }

        //GET: /cart
        public IActionResult Index()
        {
            List<CartItemModel> cartItemModelList =
                HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new();

            CartViewModel cartViewModel = new CartViewModel
            {
                CartItems = cartItemModelList,
                GrandTotal = cartItemModelList.Sum(cart => cart.Price * cart.Quantity)
            };

            return View(cartViewModel);
        }
    }
}
