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

        //GET: /admin/products/details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        //GET: /admin/products/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.FindAsync<ProductModel>(id);

            if (product == null)
                return NotFound();

            ViewBag.CategoryId =
                new SelectList(_context.Categories
                .OrderBy(category => category.Sorting), "Id", "Name", product.CategoryId);

            return View(product);
        }

        //POST: /admin/products/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel productModel)
        {
            ViewBag.CategoryId =
                new SelectList(_context.Categories
                .OrderBy(category => category.Sorting), "Id", "Name", productModel.CategoryId);

            if (ModelState.IsValid)
            {
                productModel.Slug = productModel.Name.ToLower().Replace(" ", "-");

                var p = await _context.Products
                    .Where(item => item.Id != id)
                    .FirstOrDefaultAsync(item => item.Slug == productModel.Slug);

                if (p != null)
                {
                    ModelState.AddModelError("", "The product already exists");

                    return View(productModel);
                }
                else
                {
                    if (productModel.ImageUpload != null)
                    {
                        var uploadDir = Path.Combine(_environment.WebRootPath, "media/products");

                        if(productModel.Image != "no-image-available.jpg" && productModel.Image != null)
                        {
                            var oldImagePath = Path.Combine(uploadDir, productModel.Image);

                            if (System.IO.File.Exists(oldImagePath))
                                System.IO.File.Delete(oldImagePath);
                        }

                        string imageName = Guid.NewGuid().ToString() + "_" + productModel.ImageUpload.FileName;
                        var filePath = Path.Combine(uploadDir, imageName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productModel.ImageUpload.CopyToAsync(fileStream);
                        }

                        productModel.Image = imageName;
                    }

                    _context.Products.Update(productModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The product has been changed!";

                    return RedirectToAction("Index");
                }
            }

            return View(productModel);
        }

        //GET: /admin/products/edit/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = "The product does not exist!";
            }
            else
            {
                var uploadDir = Path.Combine(_environment.WebRootPath, "media/products");

                if (product.Image != "no-image-available.jpg" && product.Image != null)
                {
                    var oldImagePath = Path.Combine(uploadDir, product.Image);

                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been removed!";
            }

            return RedirectToAction("Index");
        }
    }
}
