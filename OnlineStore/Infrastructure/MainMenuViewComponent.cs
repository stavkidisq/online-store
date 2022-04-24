using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly OnlineStoreDbContext _context;

        public MainMenuViewComponent(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();

            return View(pages);
        }

        private async Task<List<PageModel>> GetPagesAsync()
        {
            return await _context.Pages.OrderBy(page => page.Sorting).ToListAsync();
        }
    }
}
