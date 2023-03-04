using CmsShoppingChart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Infrastructure
{
    public class MainMenuViewComponent:ViewComponent
    {
        private readonly CmsShoppingContext _context;
        public MainMenuViewComponent(CmsShoppingContext context)
        {
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Pages>> GetPagesAsync()
        {
            return _context.pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
