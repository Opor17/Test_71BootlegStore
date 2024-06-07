using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.ViewModels
{
	public class OrdersUserViewModel
	{
		public string Id { get; set; }
		public string Tracking { get; set; }
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string UserName { get; set; }
		public string UserId { get; set; }
        public string? Image { get; set; }
		public string? SlipImage { get; set; }
		public string Address { get; set; }
		public int Quantity { get; set; }
		public bool Gender { get; set; }
		public int Price { get; set; }
		public int ShippingPrice { get; set; }
		public string Color { get; set; }
		public string Size { get; set; }
		public string Shipping { get; set; }
		public string Status { get; set; }
		public string ShippingStatus { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}
