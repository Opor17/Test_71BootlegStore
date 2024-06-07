using _71BootlegStore.Models;
using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.ViewModels
{
    public class ReportVM
    {
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        public List<OrdersUserViewModel>? OrdersUserViewModels { get; set; }

        public List<BestSealler> BestSeallers { get; set; }

        public List<WorstsellerList> WorstsellerList { get; set; }

	}
}
