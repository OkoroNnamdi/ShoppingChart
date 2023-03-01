using CmsShoppingChart.Infrastructure;
using CmsShoppingChart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly CmsShoppingContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(CmsShoppingContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //GET/Admin/products
        public async Task<IActionResult> Index(int p =1)

        {
            var pageSize = 3;
           
            var productlist = await _context.products.OrderByDescending(x => x.Id)
                                                            .Include(x=>x.Category)
                                                            .Skip( (p -1)*pageSize )
                                                            .Take(pageSize)
                                                            .ToListAsync();
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages =(int)Math.Ceiling ((decimal )_context.products.Count()/pageSize );
            return View(productlist);
        }
        // GET /Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.categories.OrderBy(x => x.Sorting), "Id", "Name");
            return View();
        }
        //POST /Admin/Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.categories.OrderBy(x => x.Sorting), "Id", "Name");
            if (ModelState.IsValid)

            {
                
                product.Slug = product.Name.ToLower().Replace(" ", "_");

                var slug = await _context.products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exist");
                    return View(product);

                }
                string imageName = "NoImage.png";

                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    imageName =Guid.NewGuid().ToString() +"_"+ product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync (fs);
                    fs.Close();

                    product.Image = imageName;

                }
               

                _context.Add (product );
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been added sucessfully";
                return RedirectToAction("Index");

            }
            return View(product);
        }
        // POST/Admin/Product/Edit
        [HttpPost ]
        [ValidateAntiForgeryToken ]
        public async Task<IActionResult> Edit(int id, Product product)
        {


            ViewBag.CategoryId = new SelectList(_context.categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);
            if (ModelState.IsValid)

            {

                product.Slug = product.Name.ToLower().Replace(" ", "_");

                var slug = await _context.products.Where(x=>x.Id !=id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exist");
                    return View(product);

                }
               

                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    if(!string.Equals (product.Image, "noImage.png"))
                    {
                        string oldImagePath = Path.Combine( uploadDir, product.Image  );
                        if (System.IO.File.Exists(oldImagePath)){
                            System.IO.File.Delete(oldImagePath);

                        }

                    }

                   string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    product.Image = imageName;

                }
               

                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been Edited sucessfully";
                return RedirectToAction("Index");

            }
            return View(product);

        }
        //Get/Admin/product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.products .FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId );
            return View(product);
        }
        //Admin/product/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product does not Exist";
            }
            else
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                if (!string.Equals(product.Image, "noImage.png"))
                {
                    string oldImagePath = Path.Combine(uploadDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);

                    }

                }

                _context.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product reemoved sucessfully";
            }
            return RedirectToAction("Index");
        }
    }
}
