using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _71BootlegStore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IQueryBuilder _queryBuilder;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(ApplicationDbContext dBContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager,
            IQueryBuilder queryBuilder,
            SignInManager<ApplicationUser> signin)
        {
            _context = dBContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _queryBuilder = queryBuilder;
            _signInManager = signin;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AuthUserVM authUserVM)
        {
            if (authUserVM.Password == authUserVM.ConfirmPassword)
            {
                try
                {
                    var result = new IdentityResult();
                    var roleresult = new IdentityResult();
                    var role = new Role();
                    var IsSuccessed = true;

                    var user = new ApplicationUser
                    {
                        UserName = authUserVM.ApplicationUser.UserName,
                        FirstName = authUserVM.ApplicationUser.FirstName,
                        LastName = authUserVM.ApplicationUser.LastName,
                        Email = authUserVM.ApplicationUser.Email,
                        Address = authUserVM.ApplicationUser.Address,
                        IsActive = true,
                    };


                    result = await _userManager.CreateAsync(user, authUserVM.Password);

                    if (result.Succeeded)
                    {
                        var roleId = "7d55e36c-8619-46b9-8cf8-d63e69a16801";

                        role = await _roleManager.FindByIdAsync(roleId);
                        if (role != null)
                        {
                            roleresult = await _userManager.AddToRoleAsync(user, role.Name);
                        }

                        var createdUser = await _userManager.FindByNameAsync(authUserVM.ApplicationUser.UserName);
                    }
                    else
                    {
                        IsSuccessed = false;
                    }
                    if (!IsSuccessed)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                        authUserVM.RoleList = _context.Roles.Select(r => new SelectListItem
                        {
                            Text = r.Name,
                            Value = r.Id
                        });

                        return View(authUserVM);
                    }

                    TempData["success"] = authUserVM.Title + " success";

                    return RedirectToAction("Index", "Login");

                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }

                authUserVM.RoleList = _context.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id
                });
            }
            else
            {
                ModelState.AddModelError("InvalidPassword", "Invalid password");
                TempData["error"] = "Invalid password";
            }
            return View(authUserVM);
        }
    }
}
