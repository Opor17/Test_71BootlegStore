using _71BootlegStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _71BootlegStore.ViewModels
{
	public class ShippingVM
	{
		public Shipping Shipping { get; set; }

		public IEnumerable<Shipping> Shippings { get; set; }

		[ValidateNever]
		public string? Title { get; set; }
	}
}
