using _71BootlegStore.Models;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(
            SignInManager<ApplicationUser> signin,
            UserManager<ApplicationUser> userManager
        )
        {
            _signInManager = signin;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> Login(string UserName, string Password, AuthUserVM authUserVM)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user != null)
            {
                if (user.IsActive)
                {
                    var result = await _signInManager.PasswordSignInAsync(UserName, Password, false, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        if (authUserVM.ApplicationUser.RememberMe)
                        {
                            Response.Cookies.Append("RememberMeCredentials", $"{UserName}|{Password}", new CookieOptions
                            {
                                Expires = DateTime.UtcNow.AddDays(30),
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.None
                            });
                        }
                        await _signInManager.SignInAsync(user, true);

                        TempData["success"] = "Login Success";

                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("InvalidPassword", "Invalid password");
                    TempData["error"] = "Invalid password";
                }

                TempData["error"] = "This domain user is inactive. Please contact the administrator.";
            }
            else
            {
                ModelState.AddModelError("InvalidUser", "Invalid user");
                TempData["error"] = "Invalid user";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }
    }
}
