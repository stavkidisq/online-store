using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure;

namespace OnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public ProductsController(OnlineStoreDbContext context)
        {
            _context = context;
        }

        //GET: /products
        public async Task<IActionResult> Index(int productPage = 1)
        {
            int pageSize = 6;

            ViewBag.ProductPageNumber = productPage;
            ViewBag.ProductPage = pageSize;
            ViewBag.ProductPageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(
                await _context.Products
                .OrderByDescending(product => product.Id)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                );
        }
    }
}
