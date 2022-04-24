using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class PagesController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public PagesController(OnlineStoreDbContext context)
        {
            _context = context;
        }
        //GET: /
        //GET: /slug
        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                return View(await _context.Pages.Where(page => page.Slug == "home").FirstOrDefaultAsync());
            }

            PageModel? page = await _context.Pages.Where(page => page.Slug == slug).FirstOrDefaultAsync();

            if(page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
