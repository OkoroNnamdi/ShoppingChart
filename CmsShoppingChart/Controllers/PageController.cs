using CmsShoppingChart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace CmsShoppingChart.Controllers
{
    public class PageController : Controller
    {
      private readonly CmsShoppingContext _context;
        public PageController( CmsShoppingContext context)
        {
            _context = context;
                
        }
        // Get/or/slug
        public async Task< IActionResult> Pages (string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return View(await _context.pages.Where (x=>x.Slug == slug).FirstOrDefaultAsync ());
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
