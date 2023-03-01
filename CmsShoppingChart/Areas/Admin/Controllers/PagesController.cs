using CmsShoppingChart.Infrastructure;
using CmsShoppingChart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly CmsShoppingContext _context;
        public PagesController(CmsShoppingContext context)
        {
        _context = context;
        }
        // Get Admin/Pages
        public async Task<IActionResult>  Index()
        {
            var pagelist = await _context.pages.OrderBy (x=>x.Sorting).ToListAsync();
            return View (pagelist);
        }
        // Get Admin/pages/5
        public async Task<IActionResult> Detail( int id)
        {
            var page = await _context.pages.SingleOrDefaultAsync (x => x.Id == id);
            if (page == null)
                return NotFound();
            return View(page);
        }
        // GET /Admin/Page/Create
        public  IActionResult Create() => View();

        //POST /Admin/Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pages page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "_");
                page.Sorting = 100;
                var slug = await _context.pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if(slug !=null)
                {
                    ModelState.AddModelError("", "The page already exist");
                    return View(page);

                }
                await _context.AddAsync(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page has been added sucessfully";
                return RedirectToAction ("Index");

            }
            return View(page);
        }
        // Get Admin/pages/5
        public async Task<IActionResult> Edit (int id)
        {
            var page = await _context.pages.FindAsync( id);
            if (page == null)
            {
                return NotFound();
            }
             
            return View(page);
        }
        //POST /Admin/Page/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Pages page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id==1?"home": page.Title.ToLower().Replace(" ", "_");
                
                var slug = await _context.pages.Where(x=>x.Id!=page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exist");
                    return View(page);

                }
                _context.Update (page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page edited sucessfully";
                return RedirectToAction("Edit", new {id =page.Id});

            }
            return View(page);
        }
        //Get/Admin/Categories/Delete
        public async Task<IActionResult >Delete(int id)
        {
            var page = await _context.pages.FindAsync (id);
            if (page ==null)
            {
                TempData["Error"] = "Page does not Exist";
            }
            else
            {
                _context.Remove(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page reemoved sucessfully";
            }
            return RedirectToAction("Index");
        }
        // Post// admin/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            foreach(var pageid in id)
            {
                var  page =  await _context.pages.FindAsync(pageid);
                page.Sorting = count;
                _context.Update(page);
                await _context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }


    }
}
