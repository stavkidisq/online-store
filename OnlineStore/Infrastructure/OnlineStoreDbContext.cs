using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Infrastructure
{
    public class OnlineStoreDbContext : DbContext
    {
        public DbSet<PageModel> Pages { get; set; }
        public OnlineStoreDbContext
            (DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}
