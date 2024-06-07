using System.ComponentModel.DataAnnotations;

namespace _71BootlegStore.Models
{
    public class Notifications
    {
        [StringLength(255)]
        [Display(Name = "user")]
        public string user { get; set; }

        [StringLength(255)]
        [Display(Name = "message")]
        public string message { get; set; }
    }
}
