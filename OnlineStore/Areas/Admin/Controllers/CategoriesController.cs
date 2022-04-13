using Microsoft.AspNetCore.Mvc;
using OnlineStore.Infrastructure;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public CategoriesController(OnlineStoreDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
