using _71BootlegStore.Data;
using _71BootlegStore.Models;
using _71BootlegStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _71BootlegStore.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roles;
        private readonly ApplicationDbContext _dbContext;
        public RoleController(RoleManager<Role> roles, ApplicationDbContext dBContext)
        {
            _roles = roles;
            _dbContext = dBContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Role> roles = _dbContext.Roles.OrderBy(r => r.Id).ToList();
            return View(roles);
        }

        public IActionResult Add()
        {
            var viewModel = new PermissionViewModel
            {
                PCClaimStore = new ClaimTypeStore() { ClaimValue = PolicyValueEnum.PC.ToString() },
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Save(Role role, ClaimTypeStore PCClaimStore, string RoleId)
        {
            var roleInDB = await _roles.FindByIdAsync(RoleId);

            if (roleInDB == null)
            {
                var result = await _roles.CreateAsync(role);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                await SaveClaim(role, PCClaimStore);

                if (result.Succeeded)
                {
                    TempData["success"] = "Created Role successfully";
                }
            }
            else
            {
                try
                {
                    var result = await _roles.UpdateAsync(roleInDB);

                    await SaveClaim(roleInDB, PCClaimStore);

                    if (result.Succeeded)
                    {
                        TempData["success"] = "Updated Role successfully";
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Role") });
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var rolesInDb = await _roles.FindByIdAsync(Id);

            if (rolesInDb == null)
            {
                TempData["error"] = "Role Not Exist";
                return NotFound("Role Not Exist");
            }

            var viewModel = new PermissionViewModel
            {
                Role = rolesInDb,
                PCClaimStore = new ClaimTypeStore() { ClaimValue = PolicyValueEnum.PC.ToString() },
            };

            await GetClaim(viewModel.Role, viewModel.PCClaimStore);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string Id)
        {
            var rolesInDb = await _roles.FindByIdAsync(Id);

            if (rolesInDb == null)
            {
                TempData["error"] = "Role Not Exist";
                return NotFound("Role Not Exist");
            }

            var viewModel = new PermissionViewModel
            {
                Role = rolesInDb,
                PCClaimStore = new ClaimTypeStore() { ClaimValue = PolicyValueEnum.PC.ToString() },
            };

            await GetClaim(viewModel.Role, viewModel.PCClaimStore);

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            var role = await _roles.FindByIdAsync(Id);

            if (role != null)
            {
                await _roles.DeleteAsync(role);

                TempData["success"] = "Delete Role successfully";
                return Json("success");
            }
            else
            {
                TempData["error"] = "Role Not Exist";
                return BadRequest("Role Not Exist");
            }
        }

        [NonAction]
        private async Task SaveClaim(Role role, ClaimTypeStore claimStore)
        {
            var roleHaveClaim = await _roles.GetClaimsAsync(role);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsAuthorize, claimStore.BoolsClaimsAuthorize, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsRole, claimStore.BoolsClaimsRole, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsProductManagement, claimStore.BoolsClaimsProductManagement, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsNews, claimStore.BoolsClaimsNews, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsOrder, claimStore.BoolsClaimsOrder, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsShipping, claimStore.BoolsClaimsShipping, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsPurchaseHistory, claimStore.BoolsClaimsPurchaseHistory, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsShippingUser, claimStore.BoolsClaimsShippingUser, roleHaveClaim);
            await SaveClaim(role, claimStore.ClaimValue, claimStore.RoleClaimsReport, claimStore.BoolsClaimsReport, roleHaveClaim);
        }

        [NonAction]
        private async Task SaveClaim(Role role, string claimValue, List<string> ClaimList, List<bool> claimCheck, IList<Claim> roleHaveClaim)
        {
            for (int i = 0; i < ClaimList.Count; i++)
            {
                var result = roleHaveClaim.SingleOrDefault(rc => rc.Type == ClaimList[i] && rc.Value == claimValue);

                if (!claimCheck[i] && result == null)
                {
                    continue;
                }

                if (claimCheck[i] && result != null)
                {
                    continue;
                }

                if (!claimCheck[i] && result != null)
                {
                    await _roles.RemoveClaimAsync(role, result);
                    continue;
                }

                await _roles.AddClaimAsync(role, new Claim(ClaimList[i], claimValue));
            }
        }

        [NonAction]
        private async Task GetClaim(Role role, ClaimTypeStore claimStore)
        {
            var roleHaveClaim = await _roles.GetClaimsAsync(role);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsAuthorize, claimStore.BoolsClaimsAuthorize = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsRole, claimStore.BoolsClaimsRole = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsProductManagement, claimStore.BoolsClaimsProductManagement = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsNews, claimStore.BoolsClaimsNews = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsOrder, claimStore.BoolsClaimsOrder = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsShipping, claimStore.BoolsClaimsShipping = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsPurchaseHistory, claimStore.BoolsClaimsPurchaseHistory = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsShippingUser, claimStore.BoolsClaimsShippingUser = new List<bool>(), roleHaveClaim);
            GetClaim(claimStore.ClaimValue, claimStore.RoleClaimsReport, claimStore.BoolsClaimsReport = new List<bool>(), roleHaveClaim);
        }


        [NonAction]
        private void GetClaim(string claimValue, List<string> ClaimList, List<bool> claimCheck, IList<Claim> roleHaveClaim)
        {
            for (int i = 0; i < ClaimList.Count; i++)
            {
                var result = roleHaveClaim.SingleOrDefault(rc => rc.Type == ClaimList[i] && rc.Value == claimValue);

                if (result == null)
                {
                    claimCheck.Add(false);
                    continue;
                }

                claimCheck.Add(true);
            }
        }
    }
}
