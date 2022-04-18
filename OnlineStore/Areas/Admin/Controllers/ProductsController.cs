using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineStoreDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(OnlineStoreDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //GET: admin/products
        public async Task<IActionResult> Index(int productPage = 1)
        {
            int pageSize = 6;

            ViewBag.ProductPageNumber = productPage;
            ViewBag.ProductPage = pageSize;
            ViewBag.ProductPageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(
                await _context.Products
                .OrderByDescending(product => product.Id)
                .Include(product => product.Category)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                );
        }

        //GET: admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = 
                new SelectList(_context.Categories.OrderBy(category => category.Sorting), "Id", "Name");
            return View();
        }

        //POST: /admin/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel productModel)
        {
            ViewBag.CategoryId =
                new SelectList(_context.Categories.OrderBy(category => category.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                productModel.Slug = productModel.Name.ToLower().Replace(" ", "-");

                var p = await _context.Products.FirstOrDefaultAsync(x => x.Slug == productModel.Slug);

                if (p != null)
                {
                    ModelState.AddModelError("", "The product already exists");

                    return View(productModel);
                }
                else
                {
                    var imageName = "no-image-available.jpg";

                    if(productModel.ImageUpload != null)
                    {
                        var uploadDir = Path.Combine(_environment.WebRootPath, "media/products");
                        imageName = Guid.NewGuid().ToString() + "_" + productModel.ImageUpload.FileName;
                        var filePath = Path.Combine(uploadDir, imageName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productModel.ImageUpload.CopyToAsync(fileStream);
                        }
                    }

                    productModel.Image = imageName;

                    _context.Products.Add(productModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The product has been added!";

                    return RedirectToAction("Index");
                }
            }

            return View(productModel);
        }
    }
}
