using _71BootlegStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _71BootlegStore.ViewModels
{
    public class ProductManagementVM
    {
        public ProductManagement ProductManagement { get; set; }
        public Order Order { get; set; }

        public IEnumerable<ProductManagement> ProductManagements { get; set; }

        [ValidateNever]
        public string? Title { get; set; }
    }
}
