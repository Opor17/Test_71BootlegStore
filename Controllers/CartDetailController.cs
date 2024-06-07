using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace _71BootlegStore.Controllers
{
    public class CartDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;
        private readonly IQueryBuilder _queryBuilder;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartDetailController(
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

            var cartDetail = _queryBuilder.GetCartDetailListWhereUserId(id);

            return View(cartDetail);
        }

        public async Task<IActionResult> Create(int id)
        {
            var orderVM = new OrderVM();

            var orderCartDetail = await _context.CartDetail.FindAsync(id);
            orderVM = new()
            {
                Title = "Edit CartDetail",
                CartDetail = orderCartDetail,
                ShippingList = _context.Shipping.Select(z => new SelectListItem
                {
                    Text = z.Name,
                })
            };

            return View(orderVM);
        }

        public async Task<IActionResult> UploadSlip()
        {
            var orderVM = new OrderVM();

            orderVM = new()
            {
                Title = "Upload Upload Slip",
            };

            return View(orderVM);
        }

        // Create
        [HttpPost, ActionName("UploadSlip")]
        public async Task<IActionResult> UploadSlip(string id, Shipping shipping, IFormFile? file)
        {
            ViewBag.userid = _userManager.GetUserId(HttpContext.User);
            id = ViewBag.userid;

            var cartDetailList = _queryBuilder.GetCartDetailList(id);
            var cartDetail = new CartDetail();

            if (cartDetailList != null)
            {
                if (file != null)
                {
                    string Image = await _fileUpload.UploadFile(file, "Slip", cartDetail.SlipImage);
                    cartDetail.SlipImage = Image;
                }
                foreach (var item in cartDetailList)
                {

                    cartDetail.SlipImage = cartDetail.SlipImage;
                    cartDetail.UpdatedAt = cartDetail.UpdatedAt;

                    _queryBuilder.SaveCartDetailList(cartDetail);

                    _context.SaveChanges();
                }
                TempData["success"] = "Updated Image Slip successfully";
                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        // Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Save(CartDetail cartDetail, int id, Shipping shipping)
        {
            if (cartDetail != null)
            {
                var orderCartDetailInDb = await _context.CartDetail.FindAsync(id);
                var productInDb = await _context.ProductManagement.FindAsync(orderCartDetailInDb.ProductId);
                var qty = 0;
                if (orderCartDetailInDb != null)
                {
                    if (cartDetail.Quantity > orderCartDetailInDb.Quantity)
                    {
                        var qtyold = cartDetail.Quantity - orderCartDetailInDb.Quantity;
                        qty = productInDb.Quantity - qtyold;
                    }
                    else if (orderCartDetailInDb.Quantity > cartDetail.Quantity)
                    {
                        var qtyold = orderCartDetailInDb.Quantity - cartDetail.Quantity;
                        qty = productInDb.Quantity + qtyold;
                    }

                    orderCartDetailInDb.Quantity = cartDetail.Quantity;
                    orderCartDetailInDb.SlipImage = cartDetail.SlipImage;
                    orderCartDetailInDb.Shipping = shipping.Name;
                    orderCartDetailInDb.UpdatedAt = cartDetail.UpdatedAt;

                    productInDb.Quantity = qty;

                    _context.CartDetail.Update(orderCartDetailInDb);
                    _context.ProductManagement.Update(productInDb);

                    _context.SaveChanges();
                    TempData["success"] = "Updated Successfully";

                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["error"] = "Created error";
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SaveCartDetail(string[] cartDetails, IFormFile? file, string Images, Shipping shipping)
        {
            if (cartDetails != null)
            {
                foreach (var item in cartDetails)
                {
                    int id = Int32.Parse(item);
                    var orderCartDetailInDb = await _context.CartDetail.FindAsync(id);
                    var order = new Order();

                    order.Tracking = "";
                    order.ProductId = orderCartDetailInDb.ProductId;
                    order.Name = orderCartDetailInDb.Name;
                    order.Shipping = orderCartDetailInDb.Shipping;
                    order.UserId = orderCartDetailInDb.UserId;
                    order.Image = orderCartDetailInDb.Image;
                    order.SlipImage = orderCartDetailInDb.SlipImage;
                    order.Quantity = orderCartDetailInDb.Quantity;
                    order.Gender = orderCartDetailInDb.Gender;
                    order.Price = orderCartDetailInDb.Price;
                    order.Color = orderCartDetailInDb.Color;
                    order.Size = orderCartDetailInDb.Size;
                    order.Status = orderCartDetailInDb.Status;
                    order.ShippingStatus = orderCartDetailInDb.ShippingStatus;

                    await _context.Orders.AddAsync(order);

                    var cartDetailDb = _context.CartDetail.FirstOrDefault(g => g.Id == id);
                    if (cartDetailDb != null)
                    {
                        _context.CartDetail.Remove(cartDetailDb);
                    }

                    _context.SaveChanges();
                }

                TempData["success"] = "Created Order successfully";

                return Json(new { redirectToUrl = Url.Action("Index", "PurchaseHistory") });
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
                var cartDetailDb = _context.CartDetail.FirstOrDefault(g => g.Id == id);
                var productInDb = _context.ProductManagement.FirstOrDefault(g => g.Id == cartDetailDb.ProductId);

                if (cartDetailDb != null)
                {
                    var qty = productInDb.Quantity + cartDetailDb.Quantity;

                    productInDb.Quantity = qty;

                    _context.CartDetail.Remove(cartDetailDb);
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
