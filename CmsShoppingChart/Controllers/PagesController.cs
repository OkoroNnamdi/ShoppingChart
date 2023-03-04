using CmsShoppingChart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace CmsShoppingChart.Controllers
{
    public class PagesController : Controller
    {
      private readonly CmsShoppingContext _context;
        public PagesController( CmsShoppingContext context)
        {
            _context = context;
                
        }
        // Get/or/slug
        public async Task< IActionResult> Page (string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return View(await _context.pages.Where (x=>x.Slug == "home").FirstOrDefaultAsync ());
            }
            var page = await _context.pages.Where (x => x.Slug == slug).FirstOrDefaultAsync ();
            if(page == null)
            {
                return NotFound ();
            }
            return View(page );
        }
    }
}
