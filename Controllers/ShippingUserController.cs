using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    public class ShippingUserController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IFileUpload _fileUpload;
		private readonly IQueryBuilder _queryBuilder;
		private readonly UserManager<ApplicationUser> _userManager;
		public ShippingUserController(
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

			var order = _queryBuilder.GetOrderUserListWhereIdWhereShippingStatus(id);

			return View(order);
		}
    }
}
