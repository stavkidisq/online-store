using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public PagesController(OnlineStoreDbContext context)
        {
            _context = context;
        }

        //GET: /admin/pages
        public async Task<IActionResult> Index()
        {
            var pages = await _context.Pages.OrderBy(p => p.Sorting).ToListAsync();

            return View(pages);
        }

        //GET: /admin/pages/details/5
        public async Task<IActionResult> Details(int id)
        {
            var pages = await _context.FindAsync<PageModel>(id);

            if (pages == null)
                return NotFound();

            return View(pages);
        }

        //GET: /admin/pages/create
        public IActionResult Create()
        {
            return View();
        }

        //POST: /admin/pages/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageModel pageModel)
        {
            if (ModelState.IsValid)
            {
                pageModel.Slug = pageModel.Title.ToLower().Replace(" ", "-");
                pageModel.Sorting = 100;

                var p = await _context.Pages.FirstOrDefaultAsync(x => x.Slug == pageModel.Slug);

                if (p != null)
                {
                    ModelState.AddModelError("", "The title already exists");

                    return View(pageModel);
                }
                else
                {
                    _context.Pages.Add(pageModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The page has been added!";

                    return RedirectToAction("Index");
                }
            }

            return View(pageModel);
        }

        //GET: /admin/pages/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pages = await _context.Pages.FindAsync(id);

            if (pages == null)
                return NotFound();

            return View(pages);
        }

        //POST: /admin/pages/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageModel pageModel)
        {
            if (ModelState.IsValid)
            {
                if(pageModel.Id == 1)
                {
                    pageModel.Slug = "home";
                }
                else
                {
                    pageModel.Slug = pageModel.Title.ToLower().Replace(" ", "-");
                }

                var p = await _context.Pages
                    .Where(page => page.Id != pageModel.Id)
                    .FirstOrDefaultAsync(x => x.Slug == pageModel.Slug);

                if (p != null)
                {
                    ModelState.AddModelError("", "The title already exists");

                    return View(pageModel);
                }
                else
                {
                    _context.Pages.Update(pageModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The page has been edited!";

                    return RedirectToAction("Edit", new { Id = pageModel.Id });
                }
            }

            return View(pageModel);
        }

        //GET: /admin/pages/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var page = await _context.FindAsync<PageModel>(id);

            if (page == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The page has been removed!";
            }

            return RedirectToAction("Index");
        }

        //POST: /admin/pages/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach(var pageId in id)
            {
                PageModel? page = await _context.Pages.FindAsync(pageId);
                if(page != null)
                {
                    page.Sorting = count;
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                    count++;
                }
            }

            return Ok();
        }

    }
}
