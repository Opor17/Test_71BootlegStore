using _71BootlegStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _71BootlegStore.ViewModels
{
	public class NewsVM
	{
		public News New { get; set; }

		public IEnumerable<News> News { get; set; }

		[ValidateNever]
		public string? Title { get; set; }
	}
}
