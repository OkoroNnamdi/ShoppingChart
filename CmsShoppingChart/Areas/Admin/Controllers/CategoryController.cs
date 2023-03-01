using CmsShoppingChart.Infrastructure;
using CmsShoppingChart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Areas.Admin.Controllers
{
    [Area ("Admin")]
    public class CategoryController : Controller
    {
        public readonly CmsShoppingContext _context;
        public CategoryController( CmsShoppingContext context)
        {
            _context = context;
        }
        public async Task < IActionResult >Index()
        {
            return View( await _context.categories.OrderBy (x=>x.Sorting ).ToListAsync ());
        }
        // GET /Admin/category/Create
        public IActionResult Create() => View();
        //POST /Admin/Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "_");
                category.Sorting = 100;
                var slug = await _context.categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exist");
                    return View(category);

                }
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page has been added sucessfully";
                return RedirectToAction("Index");

            }
            return View(category);
        }
        // Get Admin/Catogories/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //POST /Admin/Category/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "_");

                var slug = await _context.categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exist");
                    return View(category);

                }
                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The category edited sucessfully";
                return RedirectToAction("Edit", new { id = id});

            }
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Page does not Exist";
            }
            else
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page reemoved sucessfully";
            }
            return RedirectToAction("Index");
        }
        // Post// admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            foreach (var categoryid in id)
            {
                var category = await _context.categories.FindAsync(categoryid);
                category.Sorting = count;
                _context.Update(category);
                await _context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }

    }
}
