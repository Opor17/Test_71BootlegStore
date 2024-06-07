using _71BootlegStore.Models;

namespace _71BootlegStore.ViewModels
{
	public class HomeVM
	{
		public IEnumerable<News> News { get; set; }
		public IEnumerable<ProductManagement> ProductManagements { get; set; }
		public IEnumerable<ProductManagement> ProductManagementSize { get; set; }
		public IEnumerable<ProductManagement> Sizes { get; set; }
		public ProductManagement Size { get; set; }
	}
}
