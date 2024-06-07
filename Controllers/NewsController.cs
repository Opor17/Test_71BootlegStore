using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;

        public NewsController(ApplicationDbContext Context, IFileUpload fileUpload)
        {
            _context = Context;
            _fileUpload = fileUpload;
        }
        public IActionResult Index()
        {
			NewsVM newsVM = new()
			{
				News = _context.News.ToList()
			};

			return View(newsVM);
		}

        public async Task<IActionResult> Create(int id)
        {
            var newsVM = new NewsVM();

            if (id == null || id == 0)
            {
                newsVM = new()
                {
                    Title = "New News",
                    New = new News()
                };
            }
            else
            {
                var news = await _context.News.FindAsync(id);
                newsVM = new()
                {
                    Title = "Edit News",
                    New = news
                };
            }

            return View(newsVM);
        }

        // Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Save(News news, int id, IFormFile? file, string Images)
        {
            if (news != null)
            {
                var newsInDb = await _context.News.FindAsync(id);

                if (newsInDb == null)
                {
                    if (file != null)
                    {
                        string Image = await _fileUpload.UploadFile(file, "News", news.Image);
                        news.Image = Image;
                    }

                    await _context.News.AddAsync(news);
                    _context.SaveChanges();
                    TempData["success"] = "Created News successfully";

                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        if (file != null)
                        {
                            string Image = await _fileUpload.UploadFile(file, "News", news.Image);
                            news.Image = Image;
                        }

                        newsInDb.Image = news.Image;
                        newsInDb.UpdatedAt = news.UpdatedAt;
                        _context.News.Update(newsInDb);
                        _context.SaveChanges();
                        TempData["success"] = "Updated News successfully";

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
                var newsDb = _context.News.FirstOrDefault(g => g.Id == id);

                if (newsDb != null)
                {
                    _context.News.Remove(newsDb);
                    _context.SaveChanges();

                    returnMessage = "success";
                    TempData["success"] = "Delete item successfully!";
                }
            }
            return Json(returnMessage);

        }
    }
}
