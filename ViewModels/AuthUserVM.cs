using _71BootlegStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.ViewModels
{
    public class AuthUserVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Role")]
        public string? RoleId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

        [ValidateNever]
        public string? Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
