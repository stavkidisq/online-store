using Microsoft.AspNetCore.Mvc;
using OnlineStore.Infrastructure;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public ProductsController(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
