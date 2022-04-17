using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Infrastructure
{
    public class OnlineStoreDbContext : DbContext
    {
        public DbSet<PageModel> Pages { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public OnlineStoreDbContext
            (DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}
