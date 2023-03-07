using System.Security.Cryptography;
using CmsShoppingChart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingChart.Controllers
{
	public class ProductsController : Controller
	{
		private readonly CmsShoppingContext _context;
		public ProductsController(CmsShoppingContext context)
		{
			_context = context;
		}
        //Get/Product
		public async Task<IActionResult> Index(int p =1)
		{
			var pageSize = 6;

			var productlist = _context.products.OrderByDescending(x => x.Id)
															.Skip((p - 1) * pageSize)
															.Take(pageSize);
															
			ViewBag.PageNumber = p;
			ViewBag.PageRange = pageSize;
			ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.products.Count() / pageSize);
			return View(await productlist.ToListAsync());
		}
	}
}
