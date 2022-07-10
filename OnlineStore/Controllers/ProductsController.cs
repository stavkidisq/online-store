using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Authorize]
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

        //GET: /products/category
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int productPage = 1)
        {
            CategoryModel? categoryBySlug = await _context.Categories
                                            .Where(category => category.Slug == categorySlug)
                                            .FirstOrDefaultAsync();

            if(categoryBySlug == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productsByCategoryId = _context.Products.Where(product => product.CategoryId == categoryBySlug.Id);

            int pageSize = 6;

            ViewBag.ProductPageNumber = productPage;
            ViewBag.ProductPage = pageSize;
            ViewBag.CategoryName = categoryBySlug.Name;
            ViewBag.CategorySlug = categorySlug;

            ViewBag.ProductPageCount = (int)Math.Ceiling((decimal)productsByCategoryId.Count() / pageSize);

            return View(await productsByCategoryId
                        .Skip((productPage - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync());
        }
    }
}
