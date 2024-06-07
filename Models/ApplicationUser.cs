using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(255)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;

        public string ActiveStatus
        {
            get
            {
                if (IsActive)
                {
                    return @"<div class='flex items-center text-success'><i data-lucide='check-square' class='w-4 h-4 mr-2'></i>Active</div>";
                }
                else
                {
                    return @"<div class='flex items-center text-danger'><i data-lucide='check-square' class='w-4 h-4 mr-2'></i>Inactive</div>";
                }

            }
        }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }

        [MaxLength]
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}
