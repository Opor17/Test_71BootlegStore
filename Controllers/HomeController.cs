using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _71BootlegStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IQueryBuilder _queryBuilder;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            IQueryBuilder queryBuilder
        )
        {
            _logger = logger;
            _context = context;
            _queryBuilder = queryBuilder;
        }

        public IActionResult Index(ProductManagement productManagement)
        {
            var size = _queryBuilder.GetSizeList();
            HomeVM homeVM = new ();
            if (productManagement.Size == null)
            {
                homeVM = new()
                {
                    News = _context.News.Where(n => n.Status == true).ToList(),
                    ProductManagements = _context.ProductManagement.Where(p => p.Status == true).ToList(),
                    Sizes = size
                };
            } else if (productManagement.Size != null) 
            {
                homeVM = new()
                {
                    News = _context.News.Where(n => n.Status == true).ToList(),
                    ProductManagements = _context.ProductManagement.Where(p => p.Status == true && p.Size == productManagement.Size).ToList(),
                    Sizes = size
                };
            }

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
