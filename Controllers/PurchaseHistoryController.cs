using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace _71BootlegStore.Controllers
{
    public class PurchaseHistoryController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IFileUpload _fileUpload;
		private readonly IQueryBuilder _queryBuilder;
		private readonly UserManager<ApplicationUser> _userManager;

		public PurchaseHistoryController(
			ApplicationDbContext Context,
			IFileUpload fileUpload,
			IQueryBuilder queryBuilder,
			UserManager<ApplicationUser> userManager
		)
		{
			_context = Context;
			_fileUpload = fileUpload;
			_queryBuilder = queryBuilder;
			_userManager = userManager;
		}
		public IActionResult Index(string id)
        {
			ViewBag.userid = _userManager.GetUserId(HttpContext.User);
			id = ViewBag.userid;

			var order = _queryBuilder.GetOrderUserListWhereId(id);

			return View(order);
		}

        public async Task<IActionResult> Create(int id)
        {
            var orderVM = new OrderVM();

            var order = await _context.Orders.FindAsync(id);
            orderVM = new()
            {
                Title = "Edit PurchaseHistory",
                Order = order,
                ShippingList = _context.Shipping.Select(z => new SelectListItem
                {
                    Text = z.Name,
                })
            };

            return View(orderVM);
        }

        // Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Save(Order order, int id, IFormFile? file, string Images, Shipping shipping)
        {
            if (order != null)
            {
                var orderInDb = await _context.Orders.FindAsync(id);

                if (orderInDb != null)
                {
                    if (file != null)
                    {
                        string Image = await _fileUpload.UploadFile(file, "Slip", order.SlipImage);
                        order.SlipImage = Image;
                    }

                    orderInDb.SlipImage = order.SlipImage;
                    orderInDb.Shipping = shipping.Name;
                    orderInDb.UpdatedAt = order.UpdatedAt;
                    _context.Orders.Update(orderInDb);
                    _context.SaveChanges();
                    TempData["success"] = "Updated Order Successfully";

                    return RedirectToAction("Index");
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
                var orderDb = _context.Orders.FirstOrDefault(g => g.Id == id);

                if (orderDb != null)
                {
                    _context.Orders.Remove(orderDb);
                    _context.SaveChanges();

                    returnMessage = "success";
                    TempData["success"] = "Delete item successfully!";
                }
            }
            return Json(returnMessage);

        }
    }
}
