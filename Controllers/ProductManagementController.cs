using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _71BootlegStore.Controllers
{
    [Authorize]
    public class ProductManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;

        public ProductManagementController(ApplicationDbContext Context, IFileUpload fileUpload)
        {
            _context = Context;
            _fileUpload = fileUpload;
        }
        public IActionResult Index()
        {
			ProductManagementVM productManagementVM = new()
			{
                ProductManagements = _context.ProductManagement.ToList()
			};

			return View(productManagementVM);
		}

        public async Task<IActionResult> Create(int id)
        {
            var productManagementVM = new ProductManagementVM();

            if (id == null || id == 0)
            {
                productManagementVM = new()
                {
                    Title = "New Product",
                    ProductManagement = new ProductManagement()
                };
            }
            else
            {
                var productManagement = await _context.ProductManagement.FindAsync(id);
                productManagementVM = new()
                {
                    Title = "Edit Product",
                    ProductManagement = productManagement
                };
            }

            return View(productManagementVM);
        }

        // Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Save(ProductManagement productManagement, int id, IFormFile? file, string Images)
        {
            if (productManagement != null)
            {
                var productInDb = await _context.ProductManagement.FindAsync(id);

                if (productInDb == null)
                {
                    if (file != null)
                    {
                        string Image = await _fileUpload.UploadFile(file, "Product", productManagement.Image);
                        productManagement.Image = Image;
                    }

                    await _context.ProductManagement.AddAsync(productManagement);
                    _context.SaveChanges();
                    TempData["success"] = "Created Product successfully";

                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        if (file != null)
                        {
                            string Image = await _fileUpload.UploadFile(file, "Product", productManagement.Image);
                            productManagement.Image = Image;
                        }

                        productInDb.Name = productManagement.Name;
                        productInDb.Image = productManagement.Image;
                        productInDb.Quantity = productManagement.Quantity;
                        productInDb.Gender = productManagement.Gender;
                        productInDb.Price = productManagement.Price;
                        productInDb.Color = productManagement.Color;
                        productInDb.Size = productManagement.Size;
                        productInDb.Status = productManagement.Status;
                        productInDb.UpdatedAt = productManagement.UpdatedAt;
                        _context.ProductManagement.Update(productInDb);
                        _context.SaveChanges();
                        TempData["success"] = "Updated Product successfully";

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
                var productDb = _context.ProductManagement.FirstOrDefault(g => g.Id == id);

                if (productDb != null)
                {
                    await _fileUpload.Unlink(productDb.Image);
                    _context.ProductManagement.Remove(productDb);
                    _context.SaveChanges();

                    returnMessage = "success";
                    TempData["success"] = "Delete item successfully!";
                }
            }
            return Json(returnMessage);

        }
    }
}
