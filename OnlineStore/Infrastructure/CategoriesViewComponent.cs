using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly OnlineStoreDbContext _context;

        public CategoriesViewComponent(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();

            return View(categories);
        }

        private async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            return await _context.Categories.OrderBy(page => page.Sorting).ToListAsync();
        }
    }
}