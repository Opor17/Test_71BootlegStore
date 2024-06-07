using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    public class SaleProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;
        private readonly UserManager<ApplicationUser> _userManager;

        public SaleProductController(
            ApplicationDbContext Context,
            IFileUpload fileUpload,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = Context;
            _fileUpload = fileUpload;
            _userManager = userManager;
        }
        public async Task<IActionResult> Add(int id)
        {
            var productManagement = await _context.ProductManagement.FindAsync(id);

            ProductManagementVM productManagementVM = new()
            {
                ProductManagements = _context.ProductManagement.Where(i => i.Id == id).ToList(),
                ProductManagement = productManagement
            };

            return View(productManagementVM);
        }

        // Add
        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> Save(Order order, int id)
        {
            var productInDb = await _context.ProductManagement.FindAsync(id);

            if (productInDb != null)
            {
                ViewBag.userid = _userManager.GetUserId(HttpContext.User);

                var qty = productInDb.Quantity - order.Quantity;

                var cartDetails = new CartDetail();

                cartDetails.ProductId = productInDb.Id;
                cartDetails.OrderId = id;
                cartDetails.Name = productInDb.Name;
                cartDetails.UserId = ViewBag.userid;
                cartDetails.Image = productInDb.Image;
                cartDetails.Quantity = order.Quantity;
                cartDetails.Gender = productInDb.Gender;
                cartDetails.Price = productInDb.Price;
                cartDetails.Color = productInDb.Color;
                cartDetails.Size = productInDb.Size;
                cartDetails.Shipping = "";
                cartDetails.Status = "Waiting";
                cartDetails.ShippingStatus = "";

                productInDb.Quantity = qty;

                await _context.CartDetail.AddAsync(cartDetails);
                _context.ProductManagement.Update(productInDb);

                _context.SaveChanges();
                TempData["success"] = "Created Order successfully";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Created error";
            }
            return BadRequest();
        }
    }
}
