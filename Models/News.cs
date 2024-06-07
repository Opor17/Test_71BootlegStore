using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.Models
{
    public class News
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Image")]
        [ValidateNever]
        public string? Image { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; } = true;

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
