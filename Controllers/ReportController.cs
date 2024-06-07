using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Drawing;
using OfficeOpenXml;

namespace _71BootlegStore.Controllers
{
	public class ReportController : Controller
	{
        private readonly ApplicationDbContext _context;
        private readonly IQueryBuilder _queryBuilder;
        private readonly RoleManager<Role> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(ApplicationDbContext dBContext,
            IQueryBuilder queryBuilder,
            RoleManager<Role> roleManager,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = dBContext;
            _queryBuilder = queryBuilder;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(ReportVM reportVM)
		{
            var fromDate = reportVM.FromDate == DateTime.MinValue ? DateTime.Now.AddDays(-1) : reportVM.FromDate;
            var toDate = reportVM.ToDate == DateTime.MinValue ? DateTime.Now : DateTime.Now;

            reportVM.FromDate = fromDate;
            reportVM.ToDate = toDate;

            var orderList = _queryBuilder.GetOrderReport(reportVM);
            var BestSellerList = _queryBuilder.GetOrderBestSeller(reportVM);
            var WorstsellerList = _queryBuilder.GetOrderWorstsellerList(reportVM);

            ReportVM viewModel = new()
            {
				WorstsellerList = WorstsellerList,
                BestSeallers = BestSellerList,
                OrdersUserViewModels = orderList,
				FromDate = fromDate,
				ToDate = toDate,
			};

            return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> ReportOrderExcel(ReportVM reportVM)
		{
			var fromDate = reportVM.FromDate == DateTime.MinValue ? DateTime.Now.AddDays(-1) : reportVM.FromDate;
			var toDate = reportVM.ToDate == DateTime.MinValue ? DateTime.Now : DateTime.Now;

			reportVM.FromDate = fromDate;
			reportVM.ToDate = toDate;

			var orderList = _queryBuilder.GetOrderReport(reportVM);
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet1");

				worksheet.Cells[1, 1].Value = "Tracking";
				worksheet.Cells[1, 2].Value = "Name";
				worksheet.Cells[1, 3].Value = "First Name";
				worksheet.Cells[1, 4].Value = "UserName";
				worksheet.Cells[1, 5].Value = "Quantity";
				worksheet.Cells[1, 6].Value = "Gender";
				worksheet.Cells[1, 7].Value = "Price";
				worksheet.Cells[1, 8].Value = "Color";
				worksheet.Cells[1, 9].Value = "Size";
				worksheet.Cells[1, 10].Value = "Shipping";
				worksheet.Cells[1, 11].Value = "Status";
				worksheet.Cells[1, 12].Value = "ShippingStatus";
				worksheet.Cells[1, 13].Value = "CreatedAt";
				worksheet.Cells[1, 14].Value = "UpdatedAt";

				int row = 2;
				string wwwRootPath = this._webHostEnvironment.WebRootPath;
				foreach (var item in orderList)
				{
					worksheet.Cells[row, 1].Value = item.Tracking;
					worksheet.Cells[row, 2].Value = item.Name;
					worksheet.Cells[row, 3].Value = item.FirstName;
					worksheet.Cells[row, 4].Value = item.UserName;
					worksheet.Cells[row, 5].Value = item.Quantity;

					if (item.Gender == true)
						worksheet.Cells[row, 6].Value = "ผู้ชาย";
					else if (item.Gender == false)
						worksheet.Cells[row, 6].Value = "ผู้หญิง";

					worksheet.Cells[row, 7].Value = item.Price;
					worksheet.Cells[row, 8].Value = item.Color;
					worksheet.Cells[row, 9].Value = item.Size;
					worksheet.Cells[row, 10].Value = item.Shipping;
					worksheet.Cells[row, 11].Value = item.Status;
					worksheet.Cells[row, 12].Value = item.ShippingStatus;
					worksheet.Cells[row, 13].Value = item.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
					worksheet.Cells[row, 14].Value = item.UpdatedAt.ToString("dd/MM/yyyy HH:mm:ss");

					row++;
				}

				var stream = new MemoryStream(package.GetAsByteArray());
				stream.Position = 0;
				string date = DateTime.Now.ToString();
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderExcel.xlsx");
			}
		}
	}
}
