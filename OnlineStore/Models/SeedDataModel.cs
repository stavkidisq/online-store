using OnlineStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models
{
    public class SeedDataModel
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context 
                = new OnlineStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<OnlineStoreDbContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }

                context.AddRange(
                    new PageModel
                    {
                        Title = "Home",
                        Slug = "home",
                        Content = "home page",
                        Sorting = 0
                    },
                    new PageModel
                    {
                        Title = "About Us",
                        Slug = "about-us",
                        Content = "about us page",
                        Sorting = 100
                    },
                    new PageModel
                    {
                        Title = "Services",
                        Slug = "services",
                        Content = "services page",
                        Sorting = 100
                    },
                    new PageModel
                    {
                        Title = "Contact Us",
                        Slug = "contect-us",
                        Content = "contect us page",
                        Sorting = 100
                    });

                context.SaveChanges();
            }
        }
    }
}
