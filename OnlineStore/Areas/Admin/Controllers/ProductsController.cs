using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        //GET: admin/products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .OrderByDescending(product => product.Id)
                .Include(product => product.Category)
                .ToListAsync());
        }

        //GET: admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = 
                new SelectList(_context.Categories.OrderBy(category => category.Sorting), "Id", "Name");
            return View();
        }
    }
}
