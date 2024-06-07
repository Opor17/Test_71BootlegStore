using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _71BootlegStore.Controllers
{
    public class SettingUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQueryBuilder _queryBuilder;
        private readonly RoleManager<Role> _roleManager;

        public SettingUserController(
            ApplicationDbContext Context,
            IFileUpload fileUpload,
            UserManager<ApplicationUser> userManager,
            IQueryBuilder queryBuilder,
            RoleManager<Role> roleManager
        )
        {
            _context = Context;
            _fileUpload = fileUpload;
            _userManager = userManager;
            _queryBuilder = queryBuilder;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.userid = _userManager.GetUserId(HttpContext.User);

            var authUserVM = new AuthUserVM();

            id = ViewBag.userid;

            var user = await _userManager.FindByIdAsync(id);
            authUserVM = new()
            {
                Title = "Edit User",
                ApplicationUser = user,
                RoleList = _context.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id
                }),
                RoleId = _queryBuilder.GetUserRole(user.Id).Id,
            };

            return View(authUserVM);
        }

        public async Task<IActionResult> View(string id)
        {
            ViewBag.userid = _userManager.GetUserId(HttpContext.User);

            var authUserVM = new AuthUserVM();

            id = ViewBag.userid;

            var user = await _userManager.FindByIdAsync(id);
            authUserVM = new()
            {
                Title = "View User",
                ApplicationUser = user,
                RoleList = _context.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id
                }),
                RoleId = _queryBuilder.GetUserRole(user.Id).Id,
            };

            return View(authUserVM);
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> UpSert(AuthUserVM authUserVM)
        {
            if (authUserVM.Password == authUserVM.ConfirmPassword)
            {
                var userInDb = new ApplicationUser();

                if (authUserVM.Title == "Edit User")
                {
                    userInDb = await _userManager.FindByIdAsync(authUserVM.ApplicationUser.Id);
                    if (authUserVM.Password == null)
                    {
                        ModelState.Remove("Password");
                        ModelState.Remove("ConfirmPassword");
                    }
                }
                try
                {
                    var result = new IdentityResult();
                    var roleresult = new IdentityResult();
                    var role = new Role();
                    var IsSuccessed = true;

                    if (authUserVM.Title == "Edit User")
                    {
                        if (userInDb != null)
                        {
                            userInDb.UserName = authUserVM.ApplicationUser.UserName;
                            userInDb.FirstName = authUserVM.ApplicationUser.FirstName;
                            userInDb.LastName = authUserVM.ApplicationUser.LastName;
                            userInDb.Email = authUserVM.ApplicationUser.Email;
                            userInDb.IsActive = authUserVM.ApplicationUser.IsActive;
                            userInDb.Address = authUserVM.ApplicationUser.Address;

                            if (authUserVM.Password != null)
                                userInDb.PasswordHash = _userManager.PasswordHasher.HashPassword(userInDb, authUserVM.Password);
                        }
                        result = await _userManager.UpdateAsync(userInDb);

                        if (result.Succeeded)
                        {
                            role = await _roleManager.FindByIdAsync(authUserVM.RoleId);
                            if (role != null)
                            {
                                var currentRoles = await _userManager.GetRolesAsync(userInDb);
                                if (currentRoles.Count() > 0)
                                {
                                    await _userManager.RemoveFromRolesAsync(userInDb, currentRoles);
                                }

                                roleresult = await _userManager.AddToRoleAsync(userInDb, role.Name);
                            }
                        }
                        else
                        {
                            IsSuccessed = false;
                        }
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

                    return RedirectToAction("Index", "Home");

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
