using CmsShoppingChart.Infrastructure;
using CmsShoppingChart.Models;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Services
{
    public  class SeedData
    {
        public static void Initialize(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CmsShoppingContext>();
                if (context.pages.Any())
                {
                    return;
                }
                context.pages.AddRange(

                   new Pages
                   {
                       Title = "Home",
                       Slug = "home",
                       Content = "home page",
                       Sorting = 0
                   },
                    new Pages
                    {
                        Title = "About us",
                        Slug = "about-us",
                        Content = "about us page",
                        Sorting = 100
                    },
                    new Pages
                    {
                        Title = "Services",
                        Slug = "services",
                        Content = "services page",
                        Sorting = 100
                    },
                    new Pages
                    {
                        Title = "Contact",
                        Slug = "contact",
                        Content = "contact page",
                        Sorting = 100
                    }
                    );
                context.SaveChanges();



            }
        }
    }
}
