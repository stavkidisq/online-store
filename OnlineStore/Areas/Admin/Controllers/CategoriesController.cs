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

        //GET: /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }
        //POST: /admin/categories/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                categoryModel.Slug = categoryModel.Name.ToLower().Replace(" ", "-");

                var existCategory = await _context.Categories
                    .Where(page => page.Id != categoryModel.Id)
                    .FirstOrDefaultAsync(x => x.Slug == categoryModel.Slug);

                if (existCategory != null)
                {
                    ModelState.AddModelError("", "The category already exists");

                    return View(categoryModel);
                }
                else
                {
                    _context.Categories.Update(categoryModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The category has been edited!";

                    return RedirectToAction("Edit", new { Id = categoryModel.Id });
                }
            }

            return View(categoryModel);
        }

        //GET: /admin/categories/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                TempData["Error"] = "The category does not exist!";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The category has been removed!";
            }

            return RedirectToAction("Index");
        }

        //POST: /admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                CategoryModel? category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    category.Sorting = count;
                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();
                    count++;
                }
            }

            return Ok();
        }

    }
}
