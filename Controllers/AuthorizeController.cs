using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.Repository.IRepository;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _71BootlegStore.Controllers
{
    [Authorize]
    public class AuthorizeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IQueryBuilder _queryBuilder;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthorizeController(ApplicationDbContext dBContext,
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
            var users = _queryBuilder.GetAuthUserList();

            return View(users);
        }

        public async Task<IActionResult> Create(string id)
        {
            var authUserVM = new AuthUserVM();

            if (string.IsNullOrEmpty(id))
            {
                authUserVM = new()
                {
                    Title = "New User",
                    ApplicationUser = new ApplicationUser(),
                    RoleList = _context.Roles.Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Id
                    })
                };
            }
            else
            {
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
            }

            return View(authUserVM);
        }

        [HttpPost, ActionName("Create")]
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

                    if (authUserVM.Title == "New User")
                    {
                        var user = new ApplicationUser
                        {
                            UserName = authUserVM.ApplicationUser.UserName,
                            FirstName = authUserVM.ApplicationUser.FirstName,
                            LastName = authUserVM.ApplicationUser.LastName,
                            Email = authUserVM.ApplicationUser.Email,
                            IsActive = authUserVM.ApplicationUser.IsActive,
                            Address = authUserVM.ApplicationUser.Address,
                        };


                        result = await _userManager.CreateAsync(user, authUserVM.Password);

                        if (result.Succeeded)
                        {
                            role = await _roleManager.FindByIdAsync(authUserVM.RoleId);
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
                    }
                    else
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

                    return RedirectToAction("Index");

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

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var authUser = _context.applicationUsers.FirstOrDefault(a => a.Id == id);
                if (authUser != null)
                {
                    _context.applicationUsers.Remove(authUser);
                    _context.SaveChanges();
                }
            }

            TempData["success"] = "Deleted user successfully.";
            return Json("delete success");
        }
    }
}
