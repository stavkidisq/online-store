using Microsoft.AspNetCore.Mvc;
using OnlineStore.Infrastructure;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public CartController(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
