using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Infrastructure
{
    public class OnlineStoreDbContext : IdentityDbContext<AppUserModel>
    {
        public DbSet<PageModel> Pages { get; set; } = null!;
        public DbSet<CategoryModel> Categories { get; set; } = null!;
        public DbSet<ProductModel> Products { get; set; } = null!;
        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options) : base(options) { }
    }
}
