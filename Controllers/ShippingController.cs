using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    public class ShippingController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IFileUpload _fileUpload;

		public ShippingController(ApplicationDbContext Context, IFileUpload fileUpload)
		{
			_context = Context;
			_fileUpload = fileUpload;
		}
		public IActionResult Index()
        {
			ShippingVM shippingVM = new()
			{
				Shippings = _context.Shipping.ToList()
			};

			return View(shippingVM);
		}

		public async Task<IActionResult> Create(int id)
		{
			var shippingVM = new ShippingVM();

			if (id == null || id == 0)
			{
				shippingVM = new()
				{
					Title = "New Shipping",
					Shipping = new Shipping()
				};
			}
			else
			{
				var shipping = await _context.Shipping.FindAsync(id);
				shippingVM = new()
				{
					Title = "Edit Shipping",
					Shipping = shipping
				};
			}

			return View(shippingVM);
		}

		// Create
		[HttpPost, ActionName("Create")]
		public async Task<IActionResult> Save(Shipping shipping, int id)
		{
			if (shipping != null)
			{
				var shippingInDb = await _context.Shipping.FindAsync(id);

				if (shippingInDb == null)
				{
					await _context.Shipping.AddAsync(shipping);
					_context.SaveChanges();
					TempData["success"] = "Created Shipping successfully";

					return RedirectToAction("Index");
				}
				else
				{
					try
					{
						shippingInDb.Name = shipping.Name;
						shippingInDb.ShippingPrice = shipping.ShippingPrice;
						shippingInDb.UpdatedAt = shipping.UpdatedAt;
						_context.Shipping.Update(shippingInDb);
						_context.SaveChanges();
						TempData["success"] = "Updated Shipping successfully";

						return RedirectToAction("Index");
					}
					catch (Exception ex)
					{
						return BadRequest(ex);
					}
				}
			}
			else
			{
				TempData["error"] = "Created error";
			}

			return BadRequest();
		}

		// delete item
		[HttpDelete]
		public async Task<IActionResult> DeleteItem(int id)
		{
			var returnMessage = "error";

			if (id != 0)
			{
				var shippingDb = _context.Shipping.FirstOrDefault(g => g.Id == id);

				if (shippingDb != null)
				{
					_context.Shipping.Remove(shippingDb);
					_context.SaveChanges();

					returnMessage = "success";
					TempData["success"] = "Delete item successfully!";
				}
			}
			return Json(returnMessage);

		}
	}
}
