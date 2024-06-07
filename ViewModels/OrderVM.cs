using _71BootlegStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _71BootlegStore.ViewModels
{
	public class OrderVM
	{
		public Order Order { get; set; }

		public IEnumerable<Order> Orders { get; set; }

		public Shipping Shipping { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ShippingList { get; set; }

        public CartDetail CartDetail { get; set; }

        [ValidateNever]
		public string? Title { get; set; }

	}
}
