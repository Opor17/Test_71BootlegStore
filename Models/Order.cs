using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(255)]
        [Required]
		[Display(Name = "Tracking")]
		public string Tracking { get; set; }

		[StringLength(255)]
        [Required]
        [Display(Name = "Shipping")]
        public string? Shipping { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [Display(Name = "Image")]
        [ValidateNever]
        public string? Image { get; set; }

        [Display(Name = "SlipImage")]
        [ValidateNever]
        public string? SlipImage { get; set; }

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

        [StringLength(255)]
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [StringLength(255)]
        [Required]
        [Display(Name = "ShippingStatus")]
        public string ShippingStatus { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
