using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.Models
{
	public class Shipping
	{
		[Key]
		[Display(Name = "Id")]
		public int Id { get; set; }

		[StringLength(255)]
		[Required]
		[Display(Name = "Shipping")]
		public string Name { get; set; }

        [Display(Name = "ShippingPrice")]
        [Required]
        public int ShippingPrice { get; set; }

        [Display(Name = "CreatedAt")]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[Display(Name = "UpdatedAt")]
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}
