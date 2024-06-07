using _71BootlegStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _71BootlegStore.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ForgotPasswordController(
            UserManager<ApplicationUser> userManager
        )
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> ForgotPassword(string username, string email, string NewPassword, string ConfirmPassword)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var emailInDb = await _userManager.FindByEmailAsync(email);

                if (emailInDb != null)
                {
                    if (NewPassword != null || ConfirmPassword != null)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, NewPassword);
                        await _userManager.UpdateAsync(user);
                        TempData["success"] = "Frogot Password Success";

                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError("InvalidPassword", "Invalid Password");
                        TempData["error"] = "Invalid Password";
                    }
                }
                else
                {
                    ModelState.AddModelError("InvalidEmail", "Invalid email");
                    TempData["error"] = "Invalid email";
                }
            }
            else
            {
                ModelState.AddModelError("InvalidUsername", "Invalid username");
                TempData["error"] = "Invalid username";
            }

            return View();
        }
    }
}
