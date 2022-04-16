using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

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
        //GET: admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(category => category.Sorting).ToListAsync());
        }

        //GET: admin/categories/create
        public IActionResult Create()
        {
            return View();
        }

        //POST: /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                categoryModel.Slug = categoryModel.Name.ToLower().Replace(" ", "-");
                categoryModel.Sorting = 100;

                var p = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == categoryModel.Slug);

                if (p != null)
                {
                    ModelState.AddModelError("", "The category already exists");

                    return View(categoryModel);
                }
                else
                {
                    _context.Categories.Add(categoryModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The category has been added!";

                    return RedirectToAction("Index");
                }
            }

            return View(categoryModel);
        }
    }
}
