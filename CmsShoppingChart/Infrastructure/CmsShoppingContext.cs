using CmsShoppingChart.Models;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Infrastructure
{
    public class CmsShoppingContext:DbContext
    {
        public CmsShoppingContext(DbContextOptions<CmsShoppingContext>options):base(options)
        {

        }
        public DbSet<Pages> pages { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product > products { get; set; }
    }
}
