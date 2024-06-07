using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.Models
{
    public class ProductManagement
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        [ValidateNever]
        public string? Image { get; set; }

		[Display(Name = "Quantity")]
		[Required]
		public int Quantity { get; set; }

		[Display(Name = "Gender")]
        [Required]
        public bool Gender { get; set; }

        [Display(Name = "Price")]
        [Required]
        public int Price { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Size")]
        public string Size { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; } = true;

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
