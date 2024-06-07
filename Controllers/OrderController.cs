using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;
        private readonly IQueryBuilder _queryBuilder;

        public OrderController(
            ApplicationDbContext Context,
            IFileUpload fileUpload,
            IQueryBuilder queryBuilder
        )
        {
            _context = Context;
            _fileUpload = fileUpload;
            _queryBuilder = queryBuilder;
        }
        public IActionResult Index()
        {
            var order = _queryBuilder.GetOrderUserList();

            return View(order);
        }

        public async Task<IActionResult> Create(int id)
        {
            var orderVM = new OrderVM();

            var order = await _context.Orders.FindAsync(id);
            orderVM = new()
            {
                Title = "Edit Order",
                Order = order
            };

            return View(orderVM);
        }

        // Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Save(Order order, int id, IFormFile? file, string Images)
        {
            if (order != null)
            {
                var orderInDb = await _context.Orders.FindAsync(id);

                if (orderInDb != null)
                {
                    if (order.Status == "Cancelled")
                    {
                        orderInDb.Status = order.Status;
						orderInDb.ShippingStatus = order.ShippingStatus;
						orderInDb.UpdatedAt = order.UpdatedAt;

                        _context.Orders.Update(orderInDb);
                        _context.SaveChanges();
                        TempData["success"] = "Updated Order successfully";
                    }
                    else
                    {
                        orderInDb.Tracking = order.Tracking;
                        orderInDb.Status = order.Status;
                        orderInDb.ShippingStatus = order.ShippingStatus;
                        orderInDb.UpdatedAt = order.UpdatedAt;
                        _context.Orders.Update(orderInDb);
                        _context.SaveChanges();
                        TempData["success"] = "Updated Order successfully";
                    }

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
				var productInDb = _context.ProductManagement.FirstOrDefault(g => g.Id == orderDb.ProductId);

				if (orderDb != null)
                {
					var qty = productInDb.Quantity + orderDb.Quantity;

					productInDb.Quantity = qty;

					_context.Orders.Remove(orderDb);
					_context.ProductManagement.Update(productInDb);
					_context.SaveChanges();

                    returnMessage = "success";
                    TempData["success"] = "Delete item successfully!";
                }
            }
            return Json(returnMessage);

        }
    }
}

